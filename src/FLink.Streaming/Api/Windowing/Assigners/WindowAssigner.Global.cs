using System.Collections.Generic;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Streaming.Api.Environments;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Assigners
{
    /// <summary>
    /// A <see cref="WindowAssigner{TElement,TWindow}"/> that assigns all elements with the same key to the same single global window. This windowing scheme is only useful if you also specify a custom trigger. Otherwise, no computation will be performed, as the global window does not have a natural end at which we could process the aggregated elements.
    /// </summary>
    public class GlobalWindowAssigner<TElement> : WindowAssigner<TElement, GlobalWindow>
    {
        private GlobalWindowAssigner() { }

        public override IEnumerable<GlobalWindow> AssignWindows(TElement element, long timestamp, WindowAssignerContext context)
        {
            throw new System.NotImplementedException();
        }

        public override WindowTrigger<TElement, GlobalWindow> GetDefaultTrigger(StreamExecutionEnvironment env) => new NeverTrigger();
        public override TypeSerializer<GlobalWindow> GetWindowSerializer(ExecutionConfig executionConfig)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsEventTime => false;

        /// <summary>
        /// Creates a new <see cref="GlobalWindow"/> that assigns all elements to the same <see cref="GlobalWindow"/>.
        /// </summary>
        /// <returns></returns>
        public static GlobalWindowAssigner<TElement> Create() => new GlobalWindowAssigner<TElement>();

        public override string ToString() => "GlobalWindows()";

        /// <summary>
        /// A trigger that never fires, as default Trigger for GlobalWindows.
        /// </summary>
        public class NeverTrigger : WindowTrigger<TElement, GlobalWindow>
        {
            public override WindowTriggerResult OnElement(TElement element, long timestamp, GlobalWindow window, IWindowTriggerContext ctx)
            {
                return WindowTriggerResult.Continue;
            }

            public override WindowTriggerResult OnProcessingTime(long time, GlobalWindow window, IWindowTriggerContext ctx)
            {
                return WindowTriggerResult.Continue;
            }

            public override WindowTriggerResult OnEventTime(long time, GlobalWindow window, IWindowTriggerContext ctx)
            {
                return WindowTriggerResult.Continue;
            }

            public override void Clear(GlobalWindow window, IWindowTriggerContext ctx)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
