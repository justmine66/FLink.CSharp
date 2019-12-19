using System;
using System.Collections.Generic;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Streaming.Api.Environments;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Assigners
{
    /// <summary>
    /// A base <see cref="WindowAssigner{TElement,TWindow}"/> used to instantiate one of the deprecated operators.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    [Obsolete]
    public class BaseAlignedWindowAssigner<TElement> : WindowAssigner<TElement, TimeWindow>
    {
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
    }
}
