using System;
using System.Collections.Generic;
using FLink.Core.Exceptions;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Assigners
{
    public class SlidingEventTimeWindows<TElement> : WindowAssigner<TElement, TimeWindow>
    {
        private readonly long _size;

        private readonly long _slide;

        private readonly long _offset;

        public SlidingEventTimeWindows(long size, long slide, long offset)
        {
            if (offset < 0 || offset >= slide || size <= 0)
                throw new IllegalArgumentException("SlidingEventTimeWindows parameters must satisfy 0 <= offset < slide and size > 0");

            _size = size;
            _slide = slide;
            _offset = offset;
        }

        public override IEnumerable<TimeWindow> AssignWindows(TElement element, long timestamp, WindowAssignerContext context)
        {
            throw new System.NotImplementedException();
        }

        public override Trigger<TElement, TimeWindow> GetDefaultTrigger(StreamExecutionEnvironment env)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsEventTime => true;

        public static SlidingEventTimeWindows<TElement> Of(TimeSpan size, TimeSpan slide)
        {
            return new SlidingEventTimeWindows<TElement>((long)size.TotalMilliseconds, (long)slide.TotalMilliseconds,
                0);
        }

        public override string ToString() => "SlidingEventTimeWindows(" + _size + ", " + _slide + ")";
    }
}
