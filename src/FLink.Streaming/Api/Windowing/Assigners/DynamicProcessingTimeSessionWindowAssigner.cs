using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;
using System.Collections.Generic;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeUtils;

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

        public override IEnumerable<TimeWindow> AssignWindows(TElement element, long timestamp, WindowAssignerContext context)
        {
            throw new System.NotImplementedException();
        }

        public override WindowTrigger<TElement, TimeWindow> GetDefaultTrigger(StreamExecutionEnvironment env)
        {
            throw new System.NotImplementedException();
        }

        public override TypeSerializer<TimeWindow> GetWindowSerializer(ExecutionConfig executionConfig)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsEventTime => false;
        public override void MergeWindows(IEnumerable<TimeWindow> windows, IMergeCallback<TimeWindow> callback)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Creates a new <see cref="DynamicProcessingTimeSessionWindowAssigner{TElement}"/> that assigns elements to sessions based on the element timestamp.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sessionWindowTimeGapExtractor">The extractor to use to extract the time gap from the input elements</param>
        /// <returns>The policy.</returns>
        public static DynamicProcessingTimeSessionWindowAssigner<T> WithDynamicGap<T>(ISessionWindowTimeGapExtractor<T> sessionWindowTimeGapExtractor) => new DynamicProcessingTimeSessionWindowAssigner<T>(sessionWindowTimeGapExtractor);

        public override string ToString() => "DynamicProcessingTimeSessionWindowAssigner()";
    }
}
