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
    /// A <see cref="WindowAssigner{TElement,TWindow}"/> that windows elements into sessions based on the current processing time with a dynamic time gap. Windows cannot overlap.
    /// </summary>
    /// <typeparam name="TElement">The type of the input elements</typeparam>
    public class DynamicProcessingTimeSessionWindowAssigner<TElement> : MergingWindowAssigner<TElement, TimeWindow>
    {
        public ISessionWindowTimeGapExtractor<TElement> SessionWindowTimeGapExtractor;

        public DynamicProcessingTimeSessionWindowAssigner(ISessionWindowTimeGapExtractor<TElement> sessionWindowTimeGapExtractor)
        {
            SessionWindowTimeGapExtractor = sessionWindowTimeGapExtractor;
        }

        public override IEnumerable<TimeWindow> AssignWindows(TElement element, long timestamp,
            WindowAssignerContext context)
        {
            var currentProcessingTime = context.CurrentProcessingTime;
            var sessionTimeout = SessionWindowTimeGapExtractor.Extract(element);
            if (sessionTimeout <= 0)
                throw new IllegalArgumentException("Dynamic session time gap must satisfy 0 < gap");

            yield return new TimeWindow(currentProcessingTime, currentProcessingTime + sessionTimeout);
        }

        public override WindowTrigger<TElement, TimeWindow> GetDefaultTrigger(StreamExecutionEnvironment env) =>
            ProcessingTimeWindowTrigger<TElement>.Create();

        public override TypeSerializer<TimeWindow> GetWindowSerializer(ExecutionConfig executionConfig)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsEventTime => false;
        public override void MergeWindows(IEnumerable<TimeWindow> windows, IMergeWindowCallback<TimeWindow> callback) => TimeWindow.MergeWindows(windows, callback);
        public override string ToString() => "DynamicProcessingTimeSessionWindowAssigner()";

        /// <summary>
        /// Creates a new <see cref="DynamicProcessingTimeSessionWindowAssigner{TElement}"/> that assigns elements to sessions based on the element timestamp.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sessionWindowTimeGapExtractor">The extractor to use to extract the time gap from the input elements</param>
        /// <returns>The policy.</returns>
        public static DynamicProcessingTimeSessionWindowAssigner<T> WithDynamicGap<T>(ISessionWindowTimeGapExtractor<T> sessionWindowTimeGapExtractor) => new DynamicProcessingTimeSessionWindowAssigner<T>(sessionWindowTimeGapExtractor);
    }
}
