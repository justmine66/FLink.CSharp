using FLink.Core.Exceptions;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;
using System;
using System.Collections.Generic;

namespace FLink.Streaming.Api.Windowing.Assigners
{
    /// <summary>
    /// A <see cref="WindowAssigner{TElement,TWindow}"/> that windows elements into sliding windows based on the timestamp of the elements. Windows can possibly overlap.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public class SlidingEventTimeWindowAssigner<TElement> : WindowAssigner<TElement, TimeWindow>
    {
        private readonly long _size;

        private readonly long _slide;

        private readonly long _offset;

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

            _size = size;
            _slide = slide;
            _offset = offset;
        }

        public override IEnumerable<TimeWindow> AssignWindows(TElement element, long timestamp, WindowAssignerContext context)
        {
            throw new System.NotImplementedException();
        }

        public override WindowTrigger<TElement, TimeWindow> GetDefaultTrigger(StreamExecutionEnvironment env)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsEventTime => true;

        public static SlidingEventTimeWindowAssigner<TElement> Of(TimeSpan size, TimeSpan slide)
        {
            return new SlidingEventTimeWindowAssigner<TElement>((long)size.TotalMilliseconds, (long)slide.TotalMilliseconds,
                0);
        }

        public override string ToString() => "SlidingEventTimeWindows(" + _size + ", " + _slide + ")";
    }
}
