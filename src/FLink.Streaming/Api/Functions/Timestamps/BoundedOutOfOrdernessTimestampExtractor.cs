using System;

namespace FLink.Streaming.Api.Functions.Timestamps
{
    /// <summary>
    /// This is a <see cref="IAssignerWithPeriodicWatermarks{T}"/> used to emit Watermarks that lag behind the element with the maximum timestamp (in event time) seen so far by a fixed amount of time, <c>t_late</c>. This can help reduce the number of elements that are ignored due to lateness when computing the final result for a given window, in the case where we know that elements arrive no later than <c>t_late</c> units of time after the watermark that signals that the system event-time has advanced past their (event-time) 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BoundedOutOfOrdernessTimestampExtractor<T> : IAssignerWithPeriodicWatermarks<T>
    {
        // The current maximum timestamp seen so far.
        private long _currentMaxTimestamp;

        // The timestamp of the last emitted watermark.
        private long _lastEmittedWatermark = long.MinValue;

        // The (fixed) interval between the maximum seen timestamp seen in the records and that of the watermark to be emitted.
        public long MaxOutOfOrdernessInMillis { get; }

        protected BoundedOutOfOrdernessTimestampExtractor(TimeSpan maxOutOfOrderness)
        {
            if (maxOutOfOrderness.TotalMilliseconds < 0)
                throw new ArgumentException($"Tried to set the maximum allowed lateness to {maxOutOfOrderness}. This parameter cannot be negative.");

            MaxOutOfOrdernessInMillis = (long)maxOutOfOrderness.TotalMilliseconds;
            _currentMaxTimestamp = long.MinValue + MaxOutOfOrdernessInMillis;
        }

        /// <summary>
        /// Extracts the timestamp from the given element.
        /// </summary>
        /// <param name="element">The element that the timestamp is extracted from.</param>
        /// <returns>The new timestamp.</returns>
        public abstract long ExtractTimestamp(T element);

        public long ExtractTimestamp(T element, long previousElementTimestamp)
        {
            var timestamp = ExtractTimestamp(element);
            if (timestamp > _currentMaxTimestamp)
            {
                _currentMaxTimestamp = timestamp;
            }
            return timestamp;
        }

        /// <summary>
        /// Extracts the timestamp from the given element. The timestamp must be monotonically increasing.
        /// </summary>
        /// <param name="element">The element that the timestamp is extracted from.</param>
        /// <returns>The new timestamp.</returns>
        public abstract long ExtractAscendingTimestamp(T element);

        public Watermark.Watermark CurrentWatermark
        {
            get
            {
                // this guarantees that the watermark never goes backwards.
                var potentialWm = _currentMaxTimestamp - MaxOutOfOrdernessInMillis;
                if (potentialWm >= _lastEmittedWatermark)
                    _lastEmittedWatermark = potentialWm;

                return new Watermark.Watermark(_lastEmittedWatermark);
            }
        }
    }
}
