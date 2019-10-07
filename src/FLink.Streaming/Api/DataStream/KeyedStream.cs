using System;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.Dag;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Functions;
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
        /// Windows this <see cref="KeyedStream{T,TKey}"/> into sliding time windows.
        /// </summary>
        /// <param name="size">The size of the window.</param>
        /// <param name="slide">The slide interval.</param>
        /// <returns></returns>
        public WindowedStream<T, TKey, TimeWindow> TimeWindow(TimeSpan size, TimeSpan slide)
        {
            return null;
        }

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
