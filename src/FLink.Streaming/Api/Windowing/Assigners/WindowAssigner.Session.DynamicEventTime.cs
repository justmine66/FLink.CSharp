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
    /// A <see cref="WindowAssigner{TElement,TWindow}"/> that windows elements into sessions based on the timestamp of the elements with a dynamic time gap. Windows cannot overlap.
    /// </summary>
    /// <typeparam name="TElement">The type of the input elements</typeparam>
    public class DynamicEventTimeSessionWindowAssigner<TElement> : MergingWindowAssigner<TElement, TimeWindow>
    {
        public ISessionWindowTimeGapExtractor<TElement> SessionWindowTimeGapExtractor { get; }

        public DynamicEventTimeSessionWindowAssigner(ISessionWindowTimeGapExtractor<TElement> sessionWindowTimeGapExtractor)
        {
            SessionWindowTimeGapExtractor = sessionWindowTimeGapExtractor;
        }

        public override IEnumerable<TimeWindow> AssignWindows(TElement element, long timestamp, WindowAssignerContext context)
        {
            var sessionTimeout = SessionWindowTimeGapExtractor.Extract(element);
            if (sessionTimeout <= 0)
                throw new IllegalArgumentException("Dynamic session time gap must satisfy 0 < gap");

            yield return new TimeWindow(timestamp, timestamp + sessionTimeout);
        }

        public override WindowTrigger<TElement, TimeWindow> GetDefaultTrigger(StreamExecutionEnvironment env) =>
            EventTimeWindowTrigger<TElement>.Create();

        public override TypeSerializer<TimeWindow> GetWindowSerializer(ExecutionConfig executionConfig)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsEventTime => true;

        public override void MergeWindows(IEnumerable<TimeWindow> windows, IMergeWindowCallback<TimeWindow> callback) =>
            TimeWindow.MergeWindows(windows, callback);

        /// <summary>
        /// Creates a new <see cref="DynamicEventTimeSessionWindowAssigner{TElement}"/> that assigns elements to sessions based on the element timestamp.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sessionWindowTimeGapExtractor">The extractor to use to extract the time gap from the input elements.</param>
        /// <returns>The policy.</returns>
        public static DynamicEventTimeSessionWindowAssigner<T> WithDynamicGap<T>(ISessionWindowTimeGapExtractor<T> sessionWindowTimeGapExtractor) => new DynamicEventTimeSessionWindowAssigner<T>(sessionWindowTimeGapExtractor);

        public override string ToString() => "DynamicEventTimeSessionWindowAssigner()";
    }
}
