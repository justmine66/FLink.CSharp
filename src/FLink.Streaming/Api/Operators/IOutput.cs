using FLink.Core.Util;
using FLink.Streaming.Api.Watermarks;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// A <see cref="IStreamOperator{TOutput}"/> is supplied with an object of this interface that can be used to emit elements and other messages, such as barriers and watermarks, from an operator.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements that can be emitted.</typeparam>
    public interface IOutput<in TElement> : ICollector<TElement>
    {
        /// <summary>
        /// Emits a <see cref="Watermark"/> from an operator. This watermark is broadcast to all downstream operators.
        /// </summary>
        /// <param name="mark"></param>
        void EmitWatermark(Watermark mark);

        /// <summary>
        /// Emits a record the side output identified by the given <see cref="OutputTag{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="outputTag"></param>
        /// <param name="record">The record to collect.</param>
        void Collect<T>(OutputTag<T> outputTag, StreamRecord<T> record);

        void EmitLatencyMarker(LatencyMarker latencyMarker);
    }
}
