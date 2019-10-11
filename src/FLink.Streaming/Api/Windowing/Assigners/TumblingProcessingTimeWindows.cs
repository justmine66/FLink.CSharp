using System;
using System.Collections.Generic;
using FLink.Core.Exceptions;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Assigners
{
    public class TumblingProcessingTimeWindows<TElement> : WindowAssigner<TElement, TimeWindow>
    {
        private readonly long _size;

        private readonly long _offset;

        public TumblingProcessingTimeWindows(long size, long offset)
        {
            if (offset < 0 || offset >= size)
                throw new IllegalArgumentException("TumblingEventTimeWindows parameters must satisfy 0 <= offset < size");

            _size = size;
            _offset = offset;
        }

        public override IEnumerable<TimeWindow> AssignWindows(TElement element, long timestamp, WindowAssignerContext context)
        {
            throw new NotImplementedException();
        }

        public override Trigger<TElement, TimeWindow> GetDefaultTrigger(StreamExecutionEnvironment env)
        {
            throw new NotImplementedException();
        }

        public override bool IsEventTime => false;

        public override string ToString() => "TumblingProcessingTimeWindows(" + _size + ")";

        /// <summary>
        /// Creates a new TumblingProcessingTimeWindows, WindowAssigner that assigns elements to time windows based on the element timestamp.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static TumblingProcessingTimeWindows<TElement> Of(TimeSpan size)
        {
            return new TumblingProcessingTimeWindows<TElement>((long)size.TotalMilliseconds, 0);
        }
    }
}
