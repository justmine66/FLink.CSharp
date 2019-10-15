using System;

namespace FLink.Streaming.Api.Windowing.Windows
{
    /// <summary>
    /// A <see cref="Window"/> that represents a time interval from <see cref="Start"/> (inclusive) to <see cref="End"/>(exclusive).
    /// </summary>
    public class TimeWindow : Window, IEquatable<TimeWindow>
    {
        /// <summary>
        /// Gets the starting timestamp of the window. This is the first timestamp that belongs to this window.
        /// </summary>
        public long Start { get; }

        /// <summary>
        /// Gets the end timestamp of this window. The end timestamp is exclusive, meaning it is the first timestamp that does not belong to this window any more.
        /// </summary>
        public long End { get; }

        /// <summary>
        /// Creates a <see cref="TimeWindow"/>.
        /// </summary>
        /// <param name="startInclusive">The inclusive starting timestamp of this window.</param>
        /// <param name="endExclusive">The exclusive end timestamp of this window.</param>
        public TimeWindow(long startInclusive, long endExclusive)
        {
            Start = startInclusive;
            End = endExclusive;
        }

        /// <summary>
        /// Gets the largest timestamp that still belongs to this window.
        /// This timestamp is identical to <code><see cref="End"/> -1</code>.
        /// </summary>
        public override long MaxTimestamp => End - 1;

        /// <summary>
        /// Returns true if this window intersects the given window.
        /// </summary>
        /// <param name="other">The other window.</param>
        public bool Intersects(TimeWindow other) => Start <= other.End && End >= other.Start;

        /// <summary>
        /// Returns the minimal window covers both this window and the given window.
        /// </summary>
        /// <param name="other">The other window.</param>
        public TimeWindow Cover(TimeWindow other) =>
            new TimeWindow(Math.Min(Start, other.Start), Math.Max(End, other.End));

        public override string ToString() => "TimeWindow{" + "start=" + Start + ", end=" + End + '}';

        public bool Equals(TimeWindow other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Start == other.Start && End == other.End;
        }

        public override bool Equals(object obj) => obj is TimeWindow other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (Start.GetHashCode() * 397) ^ End.GetHashCode();
            }
        }

        #region [ Utilities ]

        /// <summary>
        /// Get the window start for a timestamp.
        /// </summary>
        /// <param name="timestamp">epoch millisecond to get the window start.</param>
        /// <param name="offset">The offset which window start would be shifted by.</param>
        /// <param name="windowSize">The size of the generated windows.</param>
        /// <returns>window start</returns>
        public static long GetWindowStartWithOffset(long timestamp, long offset, long windowSize) => timestamp - (timestamp - offset + windowSize) % windowSize;

        #endregion
    }
}
