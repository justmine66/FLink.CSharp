using System;
using System.Collections.Generic;
using FLink.Core.Exceptions;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Assigners
{
    public class TumblingEventTimeWindows<TElement> : WindowAssigner<TElement, TimeWindow>
    {
        private readonly long _size;

        private readonly long _offset;

        public TumblingEventTimeWindows(long size, long offset)
        {
            if (offset < 0 || offset >= size)
                throw new IllegalArgumentException("TumblingEventTimeWindows parameters must satisfy 0 <= offset < size");

            _size = size;
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

        public override bool IsEventTime() => true;

        /// <summary>
        /// Creates a new TumblingEventTimeWindows, WindowAssigner that assigns elements to time windows based on the element timestamp.
        /// </summary>
        /// <param name="size">The size of the generated windows.</param>
        /// <returns>The time policy.</returns>
        public static TumblingEventTimeWindows<TElement> Of(TimeSpan size)
        {
            return new TumblingEventTimeWindows<TElement>((long)size.TotalMilliseconds, 0);
        }
    }
}
