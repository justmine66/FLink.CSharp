using System;
using FLink.Streaming.Api.Watermarks;
using FLink.Streaming.Runtime.StreamStatuses;

namespace FLink.Streaming.Runtime.StreamRecords
{
    /// <summary>
    /// An element in a data stream. Can be a record or a Watermark.
    /// </summary>
    public abstract class StreamElement
    {
        /// <summary>
        /// Checks whether this element is a watermark.
        /// </summary>
        public bool IsWatermark => GetType().IsAssignableFrom(typeof(Watermark));

        /// <summary>
        /// Checks whether this element is a stream status.
        /// </summary>
        public bool IsStreamStatus => GetType().IsAssignableFrom(typeof(StreamStatus));

        /// <summary>
        /// Checks whether this element is a record.
        /// </summary>
        public bool IsRecord => GetType().IsAssignableFrom(typeof(StreamRecord<>));

        /// <summary>
        /// Checks whether this element is a latency marker.
        /// </summary>
        public bool IsLatencyMarker => GetType().IsAssignableFrom(typeof(LatencyMarker));

        /// <summary>
        /// Casts this element into a Watermark.
        /// </summary>
        /// <returns>This element as a Watermark.</returns>
        /// <exception cref="InvalidCastException">Thrown, if this element is actually not a Watermark.</exception>
        public Watermark AsWatermark() => (Watermark)this;

        /// <summary>
        /// Casts this element into a StreamRecord.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>This element as a stream record.</returns>
        /// <exception cref="InvalidCastException">Thrown, if this element is actually not a stream record.</exception>
        public StreamRecord<T> AsRecord<T>() => (StreamRecord<T>)this;

        /// <summary>
        /// Casts this element into a StreamStatus.
        /// </summary>
        /// <returns>This element as a StreamStatus.</returns>
        /// <exception cref="InvalidCastException">Thrown, if this element is actually not a stream status.</exception>
        public StreamStatus AsStreamStatus() => (StreamStatus)this;

        /// <summary>
        /// Casts this element into a LatencyMarker.
        /// </summary>
        /// <returns>This element as a LatencyMarker.</returns>
        /// <exception cref="InvalidCastException">Thrown, if this element is actually not a LatencyMarker.</exception>
        public LatencyMarker AsLatencyMarker() => (LatencyMarker)this;
    }
}
