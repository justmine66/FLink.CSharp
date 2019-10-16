using System;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Triggers
{
    /// <summary>
    /// A <see cref="WindowTrigger{TElement,TWindow}"/> that fires once the current system time passes the end of the window to which a pane belongs.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public class ProcessingTimeWindowTrigger<TElement> : WindowTrigger<TElement, TimeWindow>
    {
        private ProcessingTimeWindowTrigger() { }

        public override void Clear(TimeWindow window, ITriggerContext ctx) =>
            ctx.DeleteProcessingTimeTimer(window.MaxTimestamp);

        public override WindowTriggerResult OnElement(TElement element, long timestamp, TimeWindow window, ITriggerContext ctx)
        {
            ctx.RegisterProcessingTimeTimer(window.MaxTimestamp);
            return WindowTriggerResult.Continue;
        }

        public override WindowTriggerResult OnEventTime(long time, TimeWindow window, ITriggerContext ctx) =>
            WindowTriggerResult.Continue;

        public override WindowTriggerResult OnProcessingTime(long time, TimeWindow window, ITriggerContext ctx) =>
            WindowTriggerResult.Fire;

        public override bool CanMerge => true;

        public static ProcessingTimeWindowTrigger<TElement> Create() => new ProcessingTimeWindowTrigger<TElement>();
    }
}
