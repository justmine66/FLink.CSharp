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
    /// A WindowAssigner that windows elements into sessions based on the current processing time. Windows cannot overlap.
    /// </summary>
    public class ProcessingTimeSessionWindowAssigner<TElement> : MergingWindowAssigner<TElement, TimeWindow>
    {
        public long SessionTimeout { get; }

        protected ProcessingTimeSessionWindowAssigner(long sessionTimeout)
        {
            if (sessionTimeout <= 0)
                throw new IllegalArgumentException("the parameters must satisfy 0 < size");

            SessionTimeout = sessionTimeout;
        }

        public override IEnumerable<TimeWindow> AssignWindows(TElement element, long timestamp,
            WindowAssignerContext context)
        {
            var processingTime = context.CurrentProcessingTime;
            yield return new TimeWindow(processingTime, processingTime + SessionTimeout);
        }

        public override WindowTrigger<TElement, TimeWindow> GetDefaultTrigger(StreamExecutionEnvironment env) =>
            ProcessingTimeWindowTrigger<TElement>.Create();

        public override TypeSerializer<TimeWindow> GetWindowSerializer(ExecutionConfig executionConfig)
        {
            throw new NotImplementedException();
        }

        public override bool IsEventTime => false;

        public override void MergeWindows(IEnumerable<TimeWindow> windows, IMergeWindowCallback<TimeWindow> callback) =>
            TimeWindow.MergeWindows(windows, callback);

        public override string ToString() => "ProcessingTimeSessionWindowAssigner(" + SessionTimeout + ")";

        /// <summary>
        /// Creates a new <see cref="ProcessingTimeSessionWindowAssigner{TElement}"/> that assigns elements to sessions based on the element timestamp.
        /// </summary>
        /// <param name="size">The session timeout, i.e. the time gap between sessions</param>
        /// <returns>The policy.</returns>
        public static ProcessingTimeSessionWindowAssigner<TElement> WithGap(TimeSpan size) =>
            new ProcessingTimeSessionWindowAssigner<TElement>((long)size.TotalMilliseconds);

        /// <summary>
        /// Creates a new <see cref="DynamicProcessingTimeSessionWindowAssigner{TElement}"/> that assigns elements to sessions based on the element timestamp.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sessionWindowTimeGapExtractor">The extractor to use to extract the time gap from the input elements</param>
        /// <returns>The policy.</returns>
        public static DynamicProcessingTimeSessionWindowAssigner<T> WithDynamicGap<T>(ISessionWindowTimeGapExtractor<T> sessionWindowTimeGapExtractor) => new DynamicProcessingTimeSessionWindowAssigner<T>(sessionWindowTimeGapExtractor);
    }
}
