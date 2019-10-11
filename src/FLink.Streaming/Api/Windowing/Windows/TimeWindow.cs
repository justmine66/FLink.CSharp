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
        /// <returns></returns>
        public override long MaxTimestamp() => End - 1;

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
    }
}
