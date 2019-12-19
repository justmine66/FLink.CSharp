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
    /// A <see cref="WindowAssigner{TElement,TWindow}"/> that windows elements into sliding windows based on the current system time of the machine the operation is running on. Windows can possibly overlap.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public class SlidingProcessingTimeWindowAssigner<TElement> : WindowAssigner<TElement, TimeWindow>
    {
        public long Size;

        public long Slide;

        public long Offset;

        public SlidingProcessingTimeWindowAssigner(long size, long slide, long offset = 0)
        {
            if (offset < 0 || offset >= slide || size <= 0)
                throw new IllegalArgumentException("the parameters must satisfy 0 <= offset < slide and size > 0");

            Size = size;
            Slide = slide;
            Offset = offset;
        }

        public override IEnumerable<TimeWindow> AssignWindows(TElement element, long timestamp, WindowAssignerContext context)
        {
            timestamp = context.CurrentProcessingTime;
            var lastStart = TimeWindow.GetWindowStartWithOffset(timestamp, Offset, Slide);
            for (var start = lastStart; start > timestamp - Size; start -= Slide)
                yield return new TimeWindow(start, start + Size);
        }

        public override WindowTrigger<TElement, TimeWindow> GetDefaultTrigger(StreamExecutionEnvironment env) =>
            ProcessingTimeWindowTrigger<TElement>.Create();

        public override TypeSerializer<TimeWindow> GetWindowSerializer(ExecutionConfig executionConfig)
        {
            throw new NotImplementedException();
        }

        public override bool IsEventTime => false;

        public override string ToString() => "SlidingProcessingTimeWindowAssigner(" + Size + ", " + Slide + ", " + Offset + ")";

        /// <summary>
        /// Creates a new <see cref="SlidingProcessingTimeWindowAssigner{TElement}"/> that assigns elements to sliding time windows based on the element timestamp.
        /// </summary>
        /// <param name="size">The size of the generated windows.</param>
        /// <param name="slide">The slide interval of the generated windows.</param>
        /// <returns>The time policy.</returns>
        public static SlidingProcessingTimeWindowAssigner<TElement> Of(TimeSpan size, TimeSpan slide) => new SlidingProcessingTimeWindowAssigner<TElement>((long)size.TotalMilliseconds, (long)slide.TotalMilliseconds);

        /// <summary>
        /// Creates a new <see cref="SlidingProcessingTimeWindowAssigner{TElement}"/> that assigns elements to sliding time windows based on the element timestamp.
        /// </summary>
        /// <param name="size">The size of the generated windows.</param>
        /// <param name="slide">The slide interval of the generated windows.</param>
        /// <param name="offset">The offset which window start would be shifted by.</param>
        /// <returns>The time policy.</returns>
        public static SlidingProcessingTimeWindowAssigner<TElement>
            Of(TimeSpan size, TimeSpan slide, TimeSpan offset) => new SlidingProcessingTimeWindowAssigner<TElement>(
            (long)size.TotalMilliseconds, (long)slide.TotalMilliseconds, (long)offset.TotalMilliseconds);
    }
}
