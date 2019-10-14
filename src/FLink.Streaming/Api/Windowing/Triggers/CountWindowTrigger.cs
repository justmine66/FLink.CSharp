using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Triggers
{
    /// <summary>
    /// A <see cref="WindowTrigger{TElement,TWindow}"/> that fires once the count of elements in a pane reaches the given count.
    /// </summary>
    public class CountWindowTrigger<TElement, TWindow> : WindowTrigger<TElement, TWindow>
        where TWindow : Window
    {
        private readonly long _maxCount;

        private CountWindowTrigger(long maxCount)
        {
            _maxCount = maxCount;
        }

        public override void clear(TWindow window, ITriggerContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public override WindowTriggerResult OnElement(TElement element, long timestamp, TWindow window, ITriggerContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public override WindowTriggerResult OnEventTime(long time, TWindow window, ITriggerContext ctx) => WindowTriggerResult.Continue;

        public override WindowTriggerResult OnProcessingTime(long time, TWindow window, ITriggerContext ctx) => WindowTriggerResult.Continue;

        public override bool CanMerge => true;

        public static CountWindowTrigger<TE, TW> Of<TE, TW>(long maxCount) 
            where TW : Window
        {
            return new CountWindowTrigger<TE, TW>(maxCount);
        }
    }
}
