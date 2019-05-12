using FLink.Streaming.Runtime.StreamRecord;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// Interface for stream operators with two inputs. Use <see cref="AbstractStreamOperator"/> as a base class if you want to implement a custom operator.
    /// </summary>
    /// <typeparam name="TInput1">The first input type of the operator</typeparam>
    /// <typeparam name="TInput2">The second input type of the operator</typeparam>
    /// <typeparam name="TOutput">The output type of the operator</typeparam>
    public interface TwoInputStreamOperator<TInput1, TInput2, TOutput> : IStreamOperator<TOutput>
    {
        /// <summary>
        /// Processes one element that arrived on the first input of this two-input operator.
        /// </summary>
        /// <remarks>
        /// This method is guaranteed to not be called concurrently with other methods of the operator.
        /// </remarks>
        /// <param name="element">The <see cref="StreamRecord{T}"/></param>
        void ProcessElement1(StreamRecord<TInput1> element);

        /// <summary>
        /// Processes one element that arrived on the second input of this two-input operator.
        /// </summary>
        /// <remarks>
        /// This method is guaranteed to not be called concurrently with other methods of the operator.
        /// </remarks>
        /// <param name="element">The <see cref="StreamRecord{T}"/></param>
        void ProcessElement2(StreamRecord<TInput2> element);

        /// <summary>
        /// Processes a <see cref="Watermark.Watermark"/> that arrived on the first input of this two-input operator.
        /// </summary>
        /// <remarks>
        /// This method is guaranteed to not be called concurrently with other methods of the operator.
        /// </remarks>
        /// <param name="mark">The <see cref="Watermark.Watermark"/></param>
        void ProcessWatermark1(Watermark.Watermark mark);

        /// <summary>
        /// Processes a <see cref="Watermark.Watermark"/> that arrived on the second input of this two-input operator.
        /// </summary>
        /// <remarks>
        /// This method is guaranteed to not be called concurrently with other methods of the operator.
        /// </remarks>
        /// <param name="mark">The <see cref="Watermark.Watermark"/></param>
        void ProcessWatermark2(Watermark.Watermark mark);

        /// <summary>
        /// Processes a <see cref="LatencyMarker"/> that arrived on the first input of this two-input operator.
        /// </summary>
        /// <remarks>
        /// This method is guaranteed to not be called concurrently with other methods of the operator.
        /// </remarks>
        /// <param name="latencyMarker">The <see cref="LatencyMarker"/></param>
        void ProcessLatencyMarker1(LatencyMarker latencyMarker);

        /// <summary>
        /// Processes a <see cref="LatencyMarker"/> that arrived on the second input of this two-input operator.
        /// </summary>
        /// <remarks>
        /// This method is guaranteed to not be called concurrently with other methods of the operator.
        /// </remarks>
        /// <param name="latencyMarker">The <see cref="LatencyMarker"/></param>
        void ProcessLatencyMarker2(LatencyMarker latencyMarker);
    }
}
