using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Triggers
{
    /// <summary>
    /// A <see cref="WindowTrigger{TElement,TWindow}"/> that fires once the watermark passes the end of the window to which a pane belongs.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public class EventTimeWindowTrigger<TElement> : WindowTrigger<TElement, TimeWindow>
    {
        private EventTimeWindowTrigger() { }

        public override void Clear(TimeWindow window, IWindowTriggerContext ctx) =>
            ctx.DeleteEventTimeTimer(window.MaxTimestamp);

        public override WindowTriggerResult OnElement(TElement element, long timestamp, TimeWindow window, IWindowTriggerContext ctx)
        {
            // if the watermark is already past the window fire immediately
            if (window.MaxTimestamp <= ctx.CurrentWatermark)
                return WindowTriggerResult.Fire;

            ctx.RegisterEventTimeTimer(window.MaxTimestamp);
            return WindowTriggerResult.Continue;
        }

        public override WindowTriggerResult OnEventTime(long time, TimeWindow window, IWindowTriggerContext ctx) =>
            time == window.MaxTimestamp ? WindowTriggerResult.Fire : WindowTriggerResult.Continue;

        public override WindowTriggerResult OnProcessingTime(long time, TimeWindow window, IWindowTriggerContext ctx) =>
            WindowTriggerResult.Continue;

        public override bool CanMerge => true;

        /// <summary>
        /// Creates an event-time trigger that fires once the watermark passes the end of the window.
        /// Once the trigger fires all elements are discarded. Elements that arrive late immediately trigger window evaluation with just this one element.
        /// </summary>
        public static EventTimeWindowTrigger<TElement> Create() => new EventTimeWindowTrigger<TElement>();
    }
}