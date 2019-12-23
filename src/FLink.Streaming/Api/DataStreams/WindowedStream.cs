using System;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.States;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Core.Api.CSharp.TypeUtils;
using FLink.Core.Util;
using FLink.Streaming.Api.Functions.Windowing;
using FLink.Streaming.Api.Operators;
using FLink.Streaming.Api.Windowing.Assigners;
using FLink.Streaming.Api.Windowing.Evictors;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.DataStreams
{
    /// <summary>
    /// A <see cref="WindowedStream{T,TK,TW}"/> represents a data stream where elements are grouped by key, and for each key, the stream of elements is split into windows based on a <see cref="WindowAssigner{T,TW}"/>
    /// </summary>
    /// <typeparam name="TElement">The type of elements in the stream.</typeparam>
    /// <typeparam name="TKey">The type of the key by which elements are grouped.</typeparam>
    /// <typeparam name="TWindow">The type of <see cref="Window"/> that the <see cref="WindowAssigner{T,TW}"/> assigns the elements to.</typeparam>
    public class WindowedStream<TElement, TKey, TWindow>
        where TWindow : Window
    {
        // The keyed data stream that is windowed by this stream.
        private readonly KeyedStream<TElement, TKey> _input;
        // The window assigner.
        private readonly WindowAssigner<TElement, TWindow> _assigner;
        // The trigger that is used for window evaluation/emission.
        private WindowTrigger<TElement, TWindow> _trigger;
        // The evictor that is used for evicting elements before window evaluation.
        private IWindowEvictor<TElement, TWindow> _evictor;
        // The user-specified allowed lateness.
        private long _allowedLateness = 0L;

        public WindowedStream(KeyedStream<TElement, TKey> input, WindowAssigner<TElement, TWindow> windowAssigner)
        {
            _input = input;
            _assigner = windowAssigner;
            _trigger = windowAssigner.GetDefaultTrigger(input.ExecutionEnvironment);
        }

        /// <summary>
        /// Sets the time by which elements are allowed to be late. Elements that arrive behind the watermark by more than the specified time will be dropped. By default, the allowed lateness is 0.
        /// Setting an allowed lateness is only valid for event-time windows.
        /// </summary>
        /// <param name="lateness"> Allowed lateness specifies by how much time elements can be late before they are dropped.</param>
        /// <returns></returns>
        public WindowedStream<TElement, TKey, TWindow> AllowedLateness(TimeSpan lateness)
        {
            var millis = lateness.TotalMilliseconds;

            Preconditions.CheckArgument(millis >= 0, "The allowed lateness cannot be negative.");

            _allowedLateness = (long)millis;
            return this;
        }

        /// <summary>
        /// Send late arriving data to the side output identified by the given <see cref="OutputTag{T}"/>. Data is considered late after the watermark has passed the end of the window plus the allowed lateness set using <see cref="AllowedLateness"/>.
        /// </summary>
        /// <param name="outputTag"></param>
        /// <returns></returns>
        public WindowedStream<TElement, TKey, TWindow> SideOutputLateData(OutputTag<TElement> outputTag)
        {
            return null;
        }

        /// <summary>
        /// Sets the <see cref="IWindowEvictor{TElement,TWindow}"/> that should be used to evict elements from a window before emission.
        /// Note: When using an evictor window performance will degrade significantly, since incremental aggregation of window results cannot be used.
        /// </summary>
        /// <param name="evictor"></param>
        /// <returns></returns>
        public WindowedStream<TElement, TKey, TWindow> Evictor(IWindowEvictor<TElement, TWindow> evictor)
        {
            return this;
        }

        /// <summary>
        /// Sets the <see cref="WindowTrigger{TElement,TWindow}"/> that should be used to trigger window emission.
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public WindowedStream<TElement, TKey, TWindow> Trigger(WindowTrigger<TElement, TWindow> trigger)
        {
            return this;
        }

        /// <summary>
        /// Applies an aggregation that gives the maximum element of every window of the data stream by the given position.
        /// </summary>
        /// <param name="positionToMaxBy">The position to maximize by</param>
        /// <returns>The transformed DataStream.</returns>
        public SingleOutputStreamOperator<TElement> MaxBy(int positionToMaxBy)
        {
            return null;
        }

        #region [ Reduce Transformations ]

        /// <summary>
        /// Applies a reduce function to the window. The window function is called for each evaluation of the window for each key individually. The output of the reduce function is interpreted as a regular non-windowed stream.
        /// This window will try and incrementally aggregate data as much as the window policies permit. For example, tumbling time windows can aggregate the data, meaning that only one element per key is stored. Sliding time windows will aggregate on the granularity of the slide interval, so a few elements are stored per key (one per slide interval). Custom windows may not be able to incrementally aggregate, or may need to store extra values in an aggregation tree.
        /// </summary>
        /// <param name="function">The reduce function.</param>
        /// <returns>The data stream that is the result of applying the reduce function to the window.</returns>
        public SingleOutputStreamOperator<TElement> Reduce(IReduceFunction<TElement> function)
        {
            if (function is IRichFunction)
            {
                throw new InvalidOperationException("ReduceFunction of reduce can not be a RichFunction. " +
                    "Please use reduce(ReduceFunction, WindowFunction) instead.");
            }

            //clean the closure
            function = _input.ExecutionEnvironment.Clean(function);
            return Reduce(function, new PassThroughWindowFunction<TKey, TWindow, TElement>());
        }

        /// <summary>
        /// Applies the given window function to each window. The window function is called for each evaluation of the window for each key individually. The output of the window function is interpreted as a regular non-windowed stream.
        /// Arriving data is incrementally aggregated using the given reducer.
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="reduceFunction">The reduce function that is used for incremental aggregation.</param>
        /// <param name="function">The window function.</param>
        /// <returns>The data stream that is the result of applying the window function to the window.</returns>
        public SingleOutputStreamOperator<TOutput> Reduce<TOutput>(IReduceFunction<TElement> reduceFunction, IWindowFunction<TElement, TOutput, TKey, TWindow> function)
        {
            var inType = _input.Type;
            var resultType = getWindowFunctionReturnType(function, inType);

            return Reduce(reduceFunction, function, resultType);
        }

        /// <summary>
        /// Applies the given window function to each window. The window function is called for each evaluation of the window for each key individually. The output of the window function is interpreted as a regular non-windowed stream.
        /// Arriving data is incrementally aggregated using the given reducer.
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="reduceFunction">The reduce function that is used for incremental aggregation.</param>
        /// <param name="function">The window function.</param>
        /// <param name="resultType">Type information for the result type of the window function.</param>
        /// <returns>The data stream that is the result of applying the window function to the window.</returns>
        public SingleOutputStreamOperator<TOutput> Reduce<TOutput>(
            IReduceFunction<TElement> reduceFunction,
            IWindowFunction<TElement, TOutput, TKey, TWindow> function,
            TypeInformation<TOutput> resultType)
        {
            if (reduceFunction is IRichFunction)
            {
                throw new InvalidOperationException("ReduceFunction of reduce can not be a RichFunction.");
            }

            var env = _input.ExecutionEnvironment;
            var config = env.ExecutionConfig;

            function = env.Clean(function);
            reduceFunction = env.Clean(reduceFunction);

            var operatorName = GenerateOperatorName(_assigner, _trigger, _evictor, reduceFunction, function);
            var keySelector = _input.KeySelector;

            IOneInputStreamOperator<TElement, TOutput> @operator = default;

            if (_evictor != null)
            {

            }
            else
            {
                var stateDesc = new ReducingStateDescriptor<TElement>("window-contents", reduceFunction, _input.Type.CreateSerializer(config));
            }

            return _input.Transform(operatorName, resultType, @operator);
        }

        /// <summary>
        /// Applies the given window function to each window. The window function is called for each evaluation of the window for each key individually.The output of the window function is interpreted as a regular non-windowed stream.
        /// Arriving data is incrementally aggregated using the given reducer.
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="reduceFunction">The reduce function that is used for incremental aggregation.</param>
        /// <param name="function">The window function.</param>
        /// <returns>The data stream that is the result of applying the window function to the window.</returns>
        public SingleOutputStreamOperator<TOutput> Reduce<TOutput>(IReduceFunction<TElement> reduceFunction,
            ProcessWindowFunction<TElement, TOutput, TKey, TWindow> function)
        {
            return null;
        }

        #endregion

        #region [ Aggregation Transformations ]

        /// <summary>
        /// Applies the given aggregation function to each window. The aggregation function is called for each element, aggregating values incrementally and keeping the state to one accumulator per key and window.
        /// </summary>
        /// <typeparam name="TAccumulator">The type of the AggregateFunction's accumulator</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting stream, equal to the AggregateFunction's result type</typeparam>
        /// <param name="function">The aggregation function.</param>
        /// <returns>The data stream that is the result of applying the fold function to the window.</returns>
        public SingleOutputStreamOperator<TResult> Aggregate<TAccumulator, TResult>(IAggregateFunction<TElement, TAccumulator, TResult> function)
        {
            return null;
        }

        /// <summary>
        /// Applies the given window function to each window. The window function is called for each evaluation of the window for each key individually.The output of the window function is interpreted as a regular non-windowed stream.
        /// </summary>
        /// <typeparam name="TAccumulator">The type of the AggregateFunction's accumulator</typeparam>
        /// <typeparam name="TValue">The type of AggregateFunction's result, and the WindowFunction's input</typeparam>
        /// <typeparam name="TResult">The type of the elements in the resulting stream, equal to the WindowFunction's result type</typeparam>
        /// <param name="aggFunction">The aggregate function that is used for incremental aggregation.</param>
        /// <param name="windowFunction">The window function.</param>
        /// <returns>The data stream that is the result of applying the window function to the window.</returns>
        public SingleOutputStreamOperator<TResult> Aggregate<TAccumulator, TValue, TResult>(IAggregateFunction<TElement, TAccumulator, TValue> aggFunction, IWindowFunction<TValue, TResult, TKey, TWindow> windowFunction)
        {
            return null;
        }

        /// <summary>
        /// Applies an aggregation that sums every window of the data stream at the given position.
        /// </summary>
        /// <param name="positionToSum">The position in the tuple/array to sum</param>
        /// <returns>The transformed DataStream.</returns>
        public SingleOutputStreamOperator<TElement> Sum(int positionToSum)
        {
            return null;
        }

        /// <summary>
        /// Applies an aggregation that sums every window of the pojo data stream at the given field for every window.
        /// A field expression is either the name of a public field or a getter method with parentheses of the stream's underlying type.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public SingleOutputStreamOperator<TElement> Sum(string field)
        {
            return null;
        }

        #endregion

        private static TypeInformation<OUT> getWindowFunctionReturnType<IN, OUT, KEY, TW>(
            IWindowFunction<IN, OUT, KEY, TW> function,
            TypeInformation<IN> inType) where TW : Window
            => TypeExtractor.GetUnaryOperatorReturnType<IN, OUT>(
                function,
                typeof(IWindowFunction<IN, OUT, KEY, TW>),
                0,
                1,
                new int[] { 3, 0 },
                inType,
                null,
                false);

        private static String GenerateOperatorName<TE, TW>(
            WindowAssigner<TE, TW> assigner,
            WindowTrigger<TE, TW> trigger,
            IWindowEvictor<TE, TW> evictor,
            IFunction function1,
            IFunction function2) where TW : Window
        {
            return "Window(" +
                assigner + ", " +
                trigger.GetType().Name + ", " +
                (evictor == null ? "" : (evictor.GetType().Name + ", ")) +
                GenerateFunctionName(function1) +
                (function2 == null ? "" : (", " + GenerateFunctionName(function2))) +
                ")";
        }

        private static String GenerateFunctionName(IFunction function)
        {
            return function.GetType().Name;
        }
    }
}
