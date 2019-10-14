using System;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.Dag;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Functions;
using FLink.Streaming.Api.Windowing.Assigners;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.DataStream
{
    /// <summary>
    /// A <see cref="KeyedStream{T,TKey}"/> represents a <see cref="DataStream{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the Keyed Stream.</typeparam>
    /// <typeparam name="TKey">The type of the key in the Keyed Stream.</typeparam>
    public class KeyedStream<T, TKey> : DataStream<T>
    {
        public KeyedStream(StreamExecutionEnvironment environment, Transformation<T> transformation) : base(environment,
            transformation)
        {
        }

        /// <summary>
        /// Windows this <see cref="KeyedStream{T,TKey}"/> into tumbling time windows.
        /// </summary>
        /// <param name="size">The size of the window.</param>
        /// <returns></returns>
        public WindowedStream<T, TKey, TimeWindow> TimeWindow(TimeSpan size)
        {
            return Environment.TimeCharacteristic == TimeCharacteristic.ProcessingTime
                ? Window(TumblingProcessingTimeWindowAssigner<T>.Of(size))
                : Window(TumblingEventTimeWindowAssigner<T>.Of(size));
        }

        /// <summary>
        /// Windows this <see cref="KeyedStream{T,TKey}"/> into sliding time windows.
        /// </summary>
        /// <param name="size">The size of the window.</param>
        /// <param name="slide">The slide interval.</param>
        /// <returns></returns>
        public WindowedStream<T, TKey, TimeWindow> TimeWindow(TimeSpan size, TimeSpan slide)
        {
            return Environment.TimeCharacteristic == TimeCharacteristic.ProcessingTime
                ? Window(SlidingProcessingTimeWindowAssigner<T>.Of(size, slide))
                : Window(SlidingEventTimeWindowAssigner<T>.Of(size, slide));
        }

        /// <summary>
        /// Windows this KeyedStream into tumbling count windows.
        /// </summary>
        /// <param name="size">The size of the windows in number of elements.</param>
        /// <returns></returns>
        public WindowedStream<T, TKey, GlobalWindow> CountWindow(long size)
        {
            return null;
        }

        /// <summary>
        /// Windows this KeyedStream into sliding count windows.
        /// </summary>
        /// <param name="size">The size of the windows in number of elements.</param>
        /// <param name="slide">The slide interval in number of elements.</param>
        /// <returns></returns>
        public WindowedStream<T, TKey, GlobalWindow> CountWindow(long size, long slide)
        {
            return null;
        }

        /// <summary>
        /// Windows this data stream to a <see cref="WindowedStream{T,TK,TW}"/>, which evaluates windows over a key grouped stream. Elements are put into windows by a <see cref="WindowAssigner{T,TW}"/>. The grouping of elements is done both by key and by window.
        /// </summary>
        /// <typeparam name="TW"></typeparam>
        /// <param name="assigner">The WindowAssigner that assigns elements to windows.</param>
        /// <returns>The trigger windows data stream.</returns>
        public WindowedStream<T, TKey, TW> Window<TW>(WindowAssigner<T, TW> assigner)
            where TW : Window
        {
            return new WindowedStream<T, TKey, TW>(this, assigner);
        }

        #region [ Non-Windowed aggregation operations ]

        /// <summary>
        /// Applies a reduce transformation on the grouped data stream grouped on by the given key position. The <see cref="IReduceFunction{T}"/> will receive input values based on the key value. Only input values with the same key will go to the same reducer.
        /// </summary>
        /// <param name="reducer">The <see cref="IReduceFunction{T}"/> that will be called for every element of the input values with the same key.</param>
        /// <returns>The transformed DataStream.</returns>
        public SingleOutputStreamOperator<T> Reduce(IReduceFunction<T> reducer)
        {
            return null;
        }

        #endregion


        /// <summary>
        /// Applies the given <see cref="KeyedProcessFunction{TK,TI,TO}"/> on the input stream, thereby creating a transformed output stream.
        /// >The function will be called for every element in the input streams and can produce zero or more output elements. Contrary to the <see cref="DataStream{T}.FlatMap{T}"/> function, this function can also query the time and set timers. When reacting to the firing of set timers the function can directly emit elements and / or register yet more timers.
        /// </summary>
        /// <typeparam name="TR"></typeparam>
        /// <param name="keyedProcessFunction"></param>
        /// <returns></returns>
        public SingleOutputStreamOperator<TR> Process<TR>(KeyedProcessFunction<TKey, T, TR> keyedProcessFunction)
        {
            return null;
        }

        public SingleOutputStreamOperator<TR> Process<TR>(
            KeyedProcessFunction<TKey, T, TR> keyedProcessFunction,
            TypeInformation<TR> outputType)
        {
            return null;
        }
    }
}
