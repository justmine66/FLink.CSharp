using FLink.Core.Exceptions;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;
using System;
using System.Collections.Generic;

namespace FLink.Streaming.Api.Windowing.Assigners
{
    /// <summary>
    /// A <see cref="WindowAssigner{TElement,TWindow}"/> that windows elements into sliding windows based on the current system time of the machine the operation is running on. Windows can possibly overlap.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public class SlidingProcessingTimeWindowAssigner<TElement> : WindowAssigner<TElement, TimeWindow>
    {
        private readonly long _size;

        private readonly long _slide;

        private readonly long _offset;

        public SlidingProcessingTimeWindowAssigner(long size, long slide, long offset = 0)
        {
            if (offset < 0 || offset >= slide || size <= 0)
                throw new IllegalArgumentException("the parameters must satisfy 0 <= offset < slide and size > 0");

            _size = size;
            _slide = slide;
            _offset = offset;
        }

        public override IEnumerable<TimeWindow> AssignWindows(TElement element, long timestamp, WindowAssignerContext context)
        {
            throw new NotImplementedException();
        }

        public override WindowTrigger<TElement, TimeWindow> GetDefaultTrigger(StreamExecutionEnvironment env)
        {
            throw new NotImplementedException();
        }

        public override bool IsEventTime => false;

        public override string ToString() => "SlidingProcessingTimeWindows(" + _size + ", " + _slide + ")";

        public static SlidingProcessingTimeWindowAssigner<TElement> Of(TimeSpan size, TimeSpan slide)
        {
            return new SlidingProcessingTimeWindowAssigner<TElement>((long)size.TotalMilliseconds, (long)slide.TotalMilliseconds,
                0);
        }
    }
}
