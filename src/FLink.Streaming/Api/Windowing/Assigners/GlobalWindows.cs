using System.Collections.Generic;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Assigners
{
    /// <summary>
    /// A WindowAssigner that assigns all elements to the same GlobalWindow.
    /// </summary>
    public class GlobalWindows : WindowAssigner<object, GlobalWindow>
    {
        private GlobalWindows() { }

        public override IEnumerable<GlobalWindow> AssignWindows(object element, long timestamp, WindowAssignerContext context)
        {
            throw new System.NotImplementedException();
        }

        public override Trigger<object, GlobalWindow> GetDefaultTrigger(StreamExecutionEnvironment env)
        {
            return new NeverTrigger();
        }

        public override bool IsEventTime => false;

        /// <summary>
        /// Creates a new <see cref="GlobalWindow"/> that assigns all elements to the same <see cref="GlobalWindow"/>.
        /// </summary>
        /// <returns></returns>
        public static GlobalWindows Create()
        {
            return new GlobalWindows();
        }

        public override string ToString()
        {
            return "GlobalWindows()";
        }

        /// <summary>
        /// A trigger that never fires, as default Trigger for GlobalWindows.
        /// </summary>
        public class NeverTrigger : Trigger<object, GlobalWindow>
        {
            public override TriggerResult OnElement(object element, long timestamp, GlobalWindow window, ITriggerContext ctx)
            {
                return TriggerResult.Continue;
            }

            public override TriggerResult OnProcessingTime(long time, GlobalWindow window, ITriggerContext ctx)
            {
                return TriggerResult.Continue;
            }

            public override TriggerResult OnEventTime(long time, GlobalWindow window, ITriggerContext ctx)
            {
                return TriggerResult.Continue;
            }
        }
    }
}
