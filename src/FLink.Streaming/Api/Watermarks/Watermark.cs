﻿using System;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Api.Watermarks
{
    /// <summary>
    /// A Watermark tells operators that no elements with a timestamp older or equal to the watermark timestamp should arrive at the operator. Watermarks are emitted at the sources and propagate through the operators of the topology. Operators must themselves emit watermarks to downstream operators using
    /// </summary>
    public class Watermark : StreamElement, IEquatable<Watermark>
    {
        public static readonly Watermark MaxWatermark = new Watermark(long.MaxValue);

        /// <summary>
        /// The timestamp of the watermark in milliseconds.
        /// </summary>
        public long Timestamp { get; }

        /// <summary>
        /// Creates a new watermark with the given timestamp in milliseconds.
        /// </summary>
        /// <param name="timestamp">The timestamp of the watermark in milliseconds.</param>
        public Watermark(long timestamp)
        {
            Timestamp = timestamp;
        }

        public override bool Equals(object other)
        {
            return Equals(other as Watermark);
        }

        public bool Equals(Watermark other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Timestamp == other.Timestamp;
        }

        public override int GetHashCode() => (int)(Timestamp ^ (Timestamp >> 32));

        public override string ToString() => $"Watermark({Timestamp})";
    }
}
