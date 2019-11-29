using System;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Triggers
{
    /// <summary>
    /// A <see cref="WindowTrigger{TElement,TWindow}"/> that continuously fires based on a given time interval as measured by the clock of the machine on which the job is running.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <typeparam name="TWindow">The type of <see cref="Window"/> on which this trigger can operate.</typeparam>
    public class ContinuousProcessingTimeTrigger<TElement, TWindow> : WindowTrigger<TElement, TWindow>
        where TWindow : Window
    {
        public long Interval { get; }

        public ContinuousProcessingTimeTrigger(long interval)
        {
            Interval = interval;
        }

        public override WindowTriggerResult OnElement(TElement element, long timestamp, TWindow window, IWindowTriggerContext ctx)
        {
            throw new NotImplementedException();
        }

        public override WindowTriggerResult OnProcessingTime(long time, TWindow window, IWindowTriggerContext ctx)
        {
            throw new NotImplementedException();
        }

        public override WindowTriggerResult OnEventTime(long time, TWindow window, IWindowTriggerContext ctx) => WindowTriggerResult.Continue;

        public override void Clear(TWindow window, IWindowTriggerContext ctx)
        {
            throw new NotImplementedException();
        }
    }
}
