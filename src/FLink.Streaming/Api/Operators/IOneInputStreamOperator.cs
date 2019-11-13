using FLink.Streaming.Api.Watermarks;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// Interface for stream operators with one input. Use <see cref="AbstractStreamOperator"/> as a base class if you want to implement a custom operator.
    /// </summary>
    /// <typeparam name="TInput">The input type of the operator</typeparam>
    /// <typeparam name="TOutput">The output type of the operator</typeparam>
    public interface IOneInputStreamOperator<TInput, TOutput> : IStreamOperator<TOutput>
    {
        /// <summary>
        /// Processes one element that arrived at this operator.
        /// </summary>
        /// <remarks>
        /// This method is guaranteed to not be called concurrently with other methods of the operator.
        /// </remarks>
        /// <param name="element">The <see cref="StreamRecord{T}"/></param>
        void ProcessElement(StreamRecord<TInput> element);

        /// <summary>
        /// Processes a <see cref="Watermark"/>.
        /// </summary>
        /// <remarks>
        /// This method is guaranteed to not be called concurrently with other methods of the operator.
        /// </remarks>
        /// <param name="mark">The <see cref="Watermark"/></param>
        void ProcessWatermark(Watermark mark);

        /// <summary>
        /// Processes a <see cref="LatencyMarker"/>.
        /// </summary>
        /// <remarks>
        /// This method is guaranteed to not be called concurrently with other methods of the operator.
        /// </remarks>
        /// <param name="latencyMarker">The <see cref="LatencyMarker"/></param>
        void ProcessLatencyMarker(LatencyMarker latencyMarker);
    }
}
