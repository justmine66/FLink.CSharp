using System.Collections.Generic;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Assigners
{
    /// <summary>
    /// A WindowAssigner that assigns all elements to the same GlobalWindow.
    /// </summary>
    public class GlobalWindowAssigner : WindowAssigner<object, GlobalWindow>
    {
        private GlobalWindowAssigner() { }

        public override IEnumerable<GlobalWindow> AssignWindows(object element, long timestamp, WindowAssignerContext context)
        {
            throw new System.NotImplementedException();
        }

        public override WindowTrigger<object, GlobalWindow> GetDefaultTrigger(StreamExecutionEnvironment env)
        {
            return new NeverTrigger();
        }

        public override bool IsEventTime => false;

        /// <summary>
        /// Creates a new <see cref="GlobalWindow"/> that assigns all elements to the same <see cref="GlobalWindow"/>.
        /// </summary>
        /// <returns></returns>
        public static GlobalWindowAssigner Create()
        {
            return new GlobalWindowAssigner();
        }

        public override string ToString()
        {
            return "GlobalWindows()";
        }

        /// <summary>
        /// A trigger that never fires, as default Trigger for GlobalWindows.
        /// </summary>
        public class NeverTrigger : WindowTrigger<object, GlobalWindow>
        {
            public override WindowTriggerResult OnElement(object element, long timestamp, GlobalWindow window, ITriggerContext ctx)
            {
                return WindowTriggerResult.Continue;
            }

            public override WindowTriggerResult OnProcessingTime(long time, GlobalWindow window, ITriggerContext ctx)
            {
                return WindowTriggerResult.Continue;
            }

            public override WindowTriggerResult OnEventTime(long time, GlobalWindow window, ITriggerContext ctx)
            {
                return WindowTriggerResult.Continue;
            }
        }
    }
}
