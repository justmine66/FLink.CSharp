using FLink.Core.Api.Common.Functions;
using FLink.Core.Util;
using FLink.Streaming.Api.Functions.Windowing;
using FLink.Streaming.Api.Windowing.Assigners;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;
using System;

namespace FLink.Streaming.Api.DataStream
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
        private readonly WindowAssigner<TElement, TWindow> _windowAssigner;
        // The trigger that is used for window evaluation/emission.
        private WindowTrigger<TElement, TWindow> _trigger;
        // The user-specified allowed lateness.
        private long _allowedLateness = 0L;

        public WindowedStream(KeyedStream<TElement, TKey> input, WindowAssigner<TElement, TWindow> windowAssigner)
        {
            _input = input;
            _windowAssigner = windowAssigner;
            _trigger = windowAssigner.GetDefaultTrigger(input.Environment);
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

        #region [ Reduce Transformations ]

        /// <summary>
        /// Applies a reduce function to the window. The window function is called for each evaluation of the window for each key individually. The output of the reduce function is interpreted as a regular non-windowed stream.
        /// This window will try and incrementally aggregate data as much as the window policies permit. For example, tumbling time windows can aggregate the data, meaning that only one element per key is stored. Sliding time windows will aggregate on the granularity of the slide interval, so a few elements are stored per key (one per slide interval). Custom windows may not be able to incrementally aggregate, or may need to store extra values in an aggregation tree.
        /// </summary>
        /// <param name="function">The reduce function.</param>
        /// <returns>The data stream that is the result of applying the reduce function to the window.</returns>
        public SingleOutputStreamOperator<TElement> Reduce(IReduceFunction<TElement> function)
        {
            return null;
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
            return null;
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

        #endregion
    }
}
