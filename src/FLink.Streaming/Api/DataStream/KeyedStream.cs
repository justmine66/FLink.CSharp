using System;
using FLink.Core.Api.Dag;
using FLink.Streaming.Api.Environment;
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
    }
}
