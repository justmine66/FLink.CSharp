using System;
using System.Collections.Generic;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Exceptions;
using FLink.Streaming.Api.Environments;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Assigners
{
    /// <summary>
    /// A <see cref="WindowAssigner{TElement,TWindow}"/> that assigns each element to a window based on the current system time of the machine the operation is running on. Tumbling windows have a fixed size and do not overlap.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public class TumblingProcessingTimeWindowAssigner<TElement> : WindowAssigner<TElement, TimeWindow>
    {
        public long Size;

        public long Offset;

        /// <summary>
        /// Create a <see cref="TumblingProcessingTimeWindowAssigner{TElement}"/> instance.
        /// For example, without offsets hourly tumbling windows are aligned with epoch, that is you will get windows such as 1:00:00.000 - 1:59:59.999, 2:00:00.000 - 2:59:59.999 and so on. If you want to change that you can give an offset. With an offset of 15 minutes you would, for example, get 1:15:00.000 - 2:14:59.999, 2:15:00.000 - 3:14:59.999 etc.
        /// </summary>
        /// <param name="size">The static window size. For example, if you specify a tumbling window with a size of 5 minutes, the current window will be evaluated and a new window will be started every five minutes.</param>
        /// <param name="offset">The optional window offset that can be used to change the alignment of windows. An important use case for offsets is to adjust windows to timezones other than UTC-0. For example, in China you would have to specify an offset of Time.hours(-8).</param>
        public TumblingProcessingTimeWindowAssigner(long size, long offset = 0)
        {
            if (offset < 0 || offset >= size)
                throw new IllegalArgumentException("the parameters must satisfy 0 <= offset < size");

            Size = size;
            Offset = offset;
        }

        public override IEnumerable<TimeWindow> AssignWindows(TElement element, long timestamp, WindowAssignerContext context)
        {
            var now = context.CurrentProcessingTime;
            var start = TimeWindow.GetWindowStartWithOffset(now, Offset, Size);
            yield return new TimeWindow(start, start + Size);
        }

        public override WindowTrigger<TElement, TimeWindow> GetDefaultTrigger(StreamExecutionEnvironment env) =>
            ProcessingTimeWindowTrigger<TElement>.Create();

        public override TypeSerializer<TimeWindow> GetWindowSerializer(ExecutionConfig executionConfig)
        {
            throw new NotImplementedException();
        }

        public override bool IsEventTime => false;

        public override string ToString() => "TumblingProcessingTimeWindowAssigner(" + Size + "," + Offset + ")";

        /// <summary>
        /// Creates a new TumblingProcessingTimeWindows, WindowAssigner that assigns elements to time windows based on the element timestamp.
        /// </summary>
        /// <param name="size">The size of the generated windows.</param>
        /// <returns>The time policy.</returns>
        public static TumblingProcessingTimeWindowAssigner<TElement> Of(TimeSpan size) => new TumblingProcessingTimeWindowAssigner<TElement>((long)size.TotalMilliseconds);

        /// <summary>
        /// Creates a new TumblingProcessingTimeWindows, WindowAssigner that assigns elements to time windows based on the element timestamp.
        /// </summary>
        /// <param name="size">The size of the generated windows.</param>
        /// <param name="offset">The offset which window start would be shifted by.</param>
        /// <returns>The time policy.</returns>
        public static TumblingProcessingTimeWindowAssigner<TElement> Of(TimeSpan size, TimeSpan offset) => new TumblingProcessingTimeWindowAssigner<TElement>((long)size.TotalMilliseconds, (long)offset.TotalMilliseconds);
    }
}
