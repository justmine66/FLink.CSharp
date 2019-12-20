using System;
using FLink.Streaming.Api.Watermarks;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Triggers
{
    /// <summary>
    /// A <see cref="WindowTrigger{TElement,TWindow}"/> that continuously fires based on a given time interval.
    /// This fires based on <see cref="Watermark"/>.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <typeparam name="TWindow">The type of <see cref="Window"/> on which this trigger can operate.</typeparam>
    public class ContinuousEventTimeTrigger<TElement, TWindow> : WindowTrigger<TElement, TWindow> where TWindow : Window
    {
        public long Interval { get; }

        public ContinuousEventTimeTrigger(long interval)
        {
            Interval = interval;
        }

        public override WindowTriggerResult OnElement(TElement element, long timestamp, TWindow window, IWindowTriggerContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public override WindowTriggerResult OnProcessingTime(long time, TWindow window, IWindowTriggerContext ctx) => WindowTriggerResult.Continue;

        public override WindowTriggerResult OnEventTime(long time, TWindow window, IWindowTriggerContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public override void Clear(TWindow window, IWindowTriggerContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public static ContinuousEventTimeTrigger<TElement, TWindow> Of(TimeSpan interval) =>
            new ContinuousEventTimeTrigger<TElement, TWindow>((long)interval.TotalMilliseconds);
    }
}
