using FLink.Core.Api.Common.Functions;
using FLink.Streaming.Api.Functions.Windowing;
using FLink.Streaming.Api.Windowing.Assigners;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.DataStream
{
    /// <summary>
    /// A <see cref="WindowedStream{T,TK,TW}"/> represents a data stream where elements are grouped by key, and for each key, the stream of elements is split into windows based on a <see cref="WindowAssigner{T,TW}"/>
    /// </summary>
    /// <typeparam name="T">The type of elements in the stream.</typeparam>
    /// <typeparam name="TK">The type of the key by which elements are grouped.</typeparam>
    /// <typeparam name="TW">The type of <see cref="Window"/> that the <see cref="WindowAssigner{T,TW}"/> assigns the elements to.</typeparam>
    public class WindowedStream<T, TK, TW>
        where TW : Window
    {
        public SingleOutputStreamOperator<T> Reduce(IReduceFunction<T> function)
        {
            return null;
        }

        #region [ Aggregation Function ]

        /// <summary>
        /// Applies the given aggregation function to each window. The aggregation function is called for each element, aggregating values incrementally and keeping the state to one accumulator per key and window.
        /// </summary>
        /// <typeparam name="TAcc">The type of the AggregateFunction's accumulator</typeparam>
        /// <typeparam name="TR">The type of the elements in the resulting stream, equal to the AggregateFunction's result type</typeparam>
        /// <param name="function">The aggregation function.</param>
        /// <returns>The data stream that is the result of applying the fold function to the window.</returns>
        public SingleOutputStreamOperator<TR> Aggregate<TAcc, TR>(IAggregateFunction<T, TAcc, TR> function)
        {
            return null;
        }

        /// <summary>
        /// Applies the given window function to each window. The window function is called for each evaluation of the window for each key individually.The output of the window function is interpreted as a regular non-windowed stream.
        /// </summary>
        /// <typeparam name="TAcc">The type of the AggregateFunction's accumulator</typeparam>
        /// <typeparam name="TV">The type of AggregateFunction's result, and the WindowFunction's input</typeparam>
        /// <typeparam name="TR">The type of the elements in the resulting stream, equal to the WindowFunction's result type</typeparam>
        /// <param name="aggFunction">The aggregate function that is used for incremental aggregation.</param>
        /// <param name="windowFunction">The window function.</param>
        /// <returns>The data stream that is the result of applying the window function to the window.</returns>
        public SingleOutputStreamOperator<TR> Aggregate<TAcc, TV, TR>(
            IAggregateFunction<T, TAcc, TV> aggFunction,
            IWindowFunction<TV, TR, TK, TW> windowFunction)
        {
            return null;
        }

        #endregion
    }
}
