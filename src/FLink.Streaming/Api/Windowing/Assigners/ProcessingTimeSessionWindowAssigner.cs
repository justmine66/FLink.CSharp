using System.Collections.Generic;
using FLink.Core.Exceptions;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Assigners
{
    /// <summary>
    /// A WindowAssigner that windows elements into sessions based on the current processing time. Windows cannot overlap.
    /// </summary>
    public class ProcessingTimeSessionWindowAssigner<TElement> : MergingWindowAssigner<TElement, TimeWindow>
    {
        public long SessionTimeout { get; }

        protected ProcessingTimeSessionWindowAssigner(long sessionTimeout)
        {
            if (sessionTimeout <= 0)
                throw new IllegalArgumentException("ProcessingTimeSessionWindows parameters must satisfy 0 < size");

            SessionTimeout = sessionTimeout;
        }

        public override IEnumerable<TimeWindow> AssignWindows(TElement element, long timestamp, WindowAssignerContext context)
        {
            throw new System.NotImplementedException();
        }

        public override WindowTrigger<TElement, TimeWindow> GetDefaultTrigger(StreamExecutionEnvironment env)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsEventTime => false;

        public override void MergeWindows(IEnumerable<TimeWindow> windows, IMergeCallback<TimeWindow> callback)
        {
            throw new System.NotImplementedException();
        }
    }
}
