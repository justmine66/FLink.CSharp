using System;
using System.Collections.Generic;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Exceptions;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Assigners
{
    /// <summary>
    /// A <see cref="WindowAssigner{TElement,TWindow}"/> that windows elements into sessions based on the timestamp of the elements. Windows cannot overlap. The session windows assigner groups elements by sessions of activity. Session windows do not overlap and do not have a fixed start and end time, in contrast to tumbling windows and sliding windows. Instead a session window closes when it does not receive elements for a certain period of time,i.e., when a gap of inactivity occurred. A session window assigner can be configured with either a static session gap or with a session gap extractor function which defines how long the period of inactivity is. When this period expires, the current session closes and subsequent elements are assigned to a new session window.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public class EventTimeSessionWindowAssigner<TElement> : MergingWindowAssigner<TElement, TimeWindow>
    {
        public long SessionTimeout { get; }

        /// <summary>
        /// Create a <see cref="EventTimeSessionWindowAssigner{TElement}"/>
        /// </summary>
        /// <param name="sessionTimeout">The session timeout, i.e. the time gap between sessions.</param>
        protected EventTimeSessionWindowAssigner(long sessionTimeout)
        {
            if (sessionTimeout <= 0)
                throw new IllegalArgumentException("the parameters must satisfy 0 < size");

            SessionTimeout = sessionTimeout;
        }

        public override IEnumerable<TimeWindow> AssignWindows(TElement element, long timestamp,
            WindowAssignerContext context)
        {
            yield return new TimeWindow(timestamp, timestamp + SessionTimeout);
        }

        public override WindowTrigger<TElement, TimeWindow> GetDefaultTrigger(StreamExecutionEnvironment env) =>
            EventTimeWindowTrigger<TElement>.Create();

        public override TypeSerializer<TimeWindow> GetWindowSerializer(ExecutionConfig executionConfig)
        {
            throw new NotImplementedException();
        }

        public override string ToString() => "EventTimeSessionWindowAssigner(" + SessionTimeout + ")";

        /// <summary>
        /// Creates a new <see cref="EventTimeSessionWindowAssigner{TElement}"/> that assigns elements to sessions based on the element timestamp.
        /// </summary>
        /// <param name="size">The session timeout, i.e. the time gap between sessions.</param>
        /// <returns></returns>
        public static EventTimeSessionWindowAssigner<TElement> WithGap(TimeSpan size) => new EventTimeSessionWindowAssigner<TElement>((long)size.TotalMilliseconds);

        /// <summary>
        /// Creates a new <see cref="DynamicEventTimeSessionWindowAssigner{TElement}"/> that assigns elements to sessions based on the element timestamp.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sessionWindowTimeGapExtractor">The extractor to use to extract the time gap from the input elements.</param>
        /// <returns>The policy.</returns>
        public static DynamicEventTimeSessionWindowAssigner<T> WithDynamicGap<T>(ISessionWindowTimeGapExtractor<T> sessionWindowTimeGapExtractor) => new DynamicEventTimeSessionWindowAssigner<T>(sessionWindowTimeGapExtractor);

        public override bool IsEventTime => true;

        public override void MergeWindows(IEnumerable<TimeWindow> windows, IMergeWindowCallback<TimeWindow> callback) => TimeWindow.MergeWindows(windows, callback);
    }
}
