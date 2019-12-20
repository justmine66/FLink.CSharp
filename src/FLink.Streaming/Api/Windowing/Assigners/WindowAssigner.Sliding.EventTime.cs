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
    /// A <see cref="WindowAssigner{TElement,TWindow}"/> that windows elements into sliding windows based on the timestamp of the elements. Windows can possibly overlap.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public class SlidingEventTimeWindowAssigner<TElement> : WindowAssigner<TElement, TimeWindow>
    {
        public long Size { get; }

        public long Slide { get; }

        public long Offset { get; }

        /// <summary>
        /// Create a <see cref="SlidingEventTimeWindowAssigner{TElement}"/> instance.
        /// </summary>
        /// <param name="size">The window size.</param>
        /// <param name="slide">The slide parameter controls how frequently a sliding window is started. Hence, sliding windows can be overlapping if the slide is smaller than the window size. the elements may be assigned to multiple windows. For example, you could have windows of size 10 minutes that slides by 5 minutes. With this you get every 5 minutes a window that contains the events that arrived during the last 10 minutes</param>
        /// <param name="offset">The optional window offset that can be used to change the alignment of windows. An important use case for offsets is to adjust windows to timezones other than UTC-0. For example, in China you would have to specify an offset of Time.hours(-8)</param>
        public SlidingEventTimeWindowAssigner(long size, long slide, long offset = 0)
        {
            if (offset < 0 || offset >= slide || size <= 0)
                throw new IllegalArgumentException("the parameters must satisfy 0 <= offset < slide and size > 0");

            Size = size;
            Slide = slide;
            Offset = offset;
        }

        public override IEnumerable<TimeWindow> AssignWindows(TElement element, long timestamp, WindowAssignerContext context)
        {
            if (timestamp <= long.MinValue)
                throw new RuntimeException("Record has Long.MIN_VALUE timestamp (= no timestamp marker). " +
                                           "Is the time characteristic set to 'ProcessingTime', or did you forget to call " +
                                           "'DataStream.assignTimestampsAndWatermarks(...)'?");

            var lastStart = TimeWindow.GetWindowStartWithOffset(timestamp, Offset, Slide);
            for (var start = lastStart; start > timestamp - Size; start -= Slide)
                yield return new TimeWindow(start, start + Size);
        }

        public override WindowTrigger<TElement, TimeWindow> GetDefaultTrigger(StreamExecutionEnvironment env) =>
            EventTimeWindowTrigger<TElement>.Create();

        public override TypeSerializer<TimeWindow> GetWindowSerializer(ExecutionConfig executionConfig)
        {
            throw new NotImplementedException();
        }

        public override bool IsEventTime => true;

        /// <summary>
        /// Creates a new <see cref="SlidingEventTimeWindowAssigner{TElement}"/> that assigns elements to sliding time windows based on the element timestamp.
        /// </summary>
        /// <param name="size">The size of the generated windows.</param>
        /// <param name="slide">The slide interval of the generated windows.</param>
        /// <returns>The time policy.</returns>
        public static SlidingEventTimeWindowAssigner<TElement> Of(TimeSpan size, TimeSpan slide) => new SlidingEventTimeWindowAssigner<TElement>((long)size.TotalMilliseconds, (long)slide.TotalMilliseconds);

        /// <summary>
        /// Creates a new <see cref="SlidingEventTimeWindowAssigner{TElement}"/> that assigns elements to sliding time windows based on the element timestamp.
        /// </summary>
        /// <param name="size">The size of the generated windows.</param>
        /// <param name="slide">The slide interval of the generated windows.</param>
        /// <param name="offset">The offset which window start would be shifted by.</param>
        /// <returns>The time policy.</returns>
        public static SlidingEventTimeWindowAssigner<TElement> Of(TimeSpan size, TimeSpan slide, TimeSpan offset) =>
            new SlidingEventTimeWindowAssigner<TElement>((long)size.TotalMilliseconds, (long)slide.TotalMilliseconds,
                (long)offset.TotalMilliseconds);

        public override string ToString() => "SlidingEventTimeWindowAssigner(" + Size + ", " + Slide + ", " + Offset + ")";
    }
}
