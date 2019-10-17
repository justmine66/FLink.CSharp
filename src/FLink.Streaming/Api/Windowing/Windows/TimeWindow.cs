using System;
using System.Collections.Generic;
using System.Linq;
using FLink.Streaming.Api.Windowing.Assigners;

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

        /// <summary>
        /// Merge overlapping <see cref="TimeWindow"/>s. For use by merging <see cref="WindowAssigner{TElement,TWindow}"/>.
        /// </summary>
        /// <param name="windows"></param>
        /// <param name="callback"></param>
        public static void MergeWindows(IEnumerable<TimeWindow> windows, IMergeWindowCallback<TimeWindow> callback)
        {
            // sort the windows by the start time and then merge overlapping windows
            var sortedWindows = windows.OrderBy(it => it.Start);

            var merged = new List<MergedItem>();
            MergedItem currentMerge = null;

            foreach (var candidate in sortedWindows)
            {
                if (currentMerge == null)
                    currentMerge = new MergedItem(candidate, candidate);
                else if (currentMerge.MergeResult.Intersects(candidate))
                {   // 两个窗口的时间段相互重叠，则属于同一会话，因此会话窗口策略把这两个窗口合并成一个新的、更长的会话窗口。
                    currentMerge.MergeResult = currentMerge.MergeResult.Cover(candidate);
                    currentMerge.ToBeMerged.Add(candidate);
                }
                else
                {
                    merged.Add(currentMerge);
                    currentMerge = new MergedItem(candidate, candidate);
                }
            }

            if (currentMerge != null)
                merged.Add(currentMerge);

            foreach (var mergeItem in merged)
            {
                if (mergeItem.ToBeMerged.Count > 0)
                    callback.Merge(mergeItem.ToBeMerged, mergeItem.MergeResult);
            }
        }

        private class MergedItem
        {
            public MergedItem(TimeWindow window, params TimeWindow[] candidates)
            {
                MergeResult = window;
                ToBeMerged = new HashSet<TimeWindow>(candidates);
            }

            public TimeWindow MergeResult { get; set; }
            public HashSet<TimeWindow> ToBeMerged { get; }
        }

        #endregion
    }
}
