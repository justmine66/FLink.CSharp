using System;
using System.Collections.Generic;
using FLink.Core.Exceptions;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Assigners
{
    /// <summary>
    /// A WindowAssigner that windows elements into sessions based on the timestamp of the elements.Windows cannot overlap.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public class EventTimeSessionWindows<TElement> : MergingWindowAssigner<TElement, TimeWindow>
    {
        public long SessionTimeout { get; }

        protected EventTimeSessionWindows(long sessionTimeout)
        {
            if (sessionTimeout <= 0)
                throw new IllegalArgumentException("EventTimeSessionWindows parameters must satisfy 0 < size");

            SessionTimeout = sessionTimeout;
        }

        public override IEnumerable<TimeWindow> AssignWindows(TElement element, long timestamp, WindowAssignerContext context)
        {
            throw new NotImplementedException();
        }

        public override Trigger<TElement, TimeWindow> GetDefaultTrigger(StreamExecutionEnvironment env)
        {
            throw new NotImplementedException();
        }

        public override bool IsEventTime() => true;

        public override void MergeWindows(IEnumerable<TimeWindow> windows, IMergeCallback<TimeWindow> callback)
        {
            throw new NotImplementedException();
        }
    }
}
