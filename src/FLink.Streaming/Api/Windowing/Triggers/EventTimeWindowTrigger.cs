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

        public override void clear(TimeWindow window, ITriggerContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public override WindowTriggerResult OnElement(TElement element, long timestamp, TimeWindow window, ITriggerContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public override WindowTriggerResult OnEventTime(long time, TimeWindow window, ITriggerContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public override WindowTriggerResult OnProcessingTime(long time, TimeWindow window, ITriggerContext ctx) => WindowTriggerResult.Continue;

        public override bool CanMerge => true;

        /// <summary>
        /// Creates an event-time trigger that fires once the watermark passes the end of the window.
        /// Once the trigger fires all elements are discarded. Elements that arrive late immediately trigger window evaluation with just this one element.
        /// </summary>
        public static EventTimeWindowTrigger<TElement> Create() => new EventTimeWindowTrigger<TElement>();
    }
}