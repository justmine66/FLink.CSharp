using System;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Triggers
{
    /// <summary>
    /// A trigger that can turn any <see cref="WindowTrigger{TElement,TWindow}"/> into a purging <see cref="WindowTrigger{TElement,TWindow}"/>.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <typeparam name="TWindow"></typeparam>
    public class PurgingWindowTrigger<TElement, TWindow> : WindowTrigger<TElement, TWindow>
        where TWindow : Window
    {
        public WindowTrigger<TElement, TWindow> NestedTrigger;

        private PurgingWindowTrigger(WindowTrigger<TElement, TWindow> nestedTrigger)
        {
            NestedTrigger = nestedTrigger;
        }

        public override void clear(TWindow window, ITriggerContext ctx)
        {
            throw new NotImplementedException();
        }

        public override WindowTriggerResult OnElement(TElement element, long timestamp, TWindow window, ITriggerContext ctx)
        {
            throw new NotImplementedException();
        }

        public override WindowTriggerResult OnEventTime(long time, TWindow window, ITriggerContext ctx)
        {
            throw new NotImplementedException();
        }

        public override WindowTriggerResult OnProcessingTime(long time, TWindow window, ITriggerContext ctx)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new purging trigger from the given <see cref="WindowTrigger{TElement,TWindow}"/>.
        /// </summary>
        /// <typeparam name="TE"></typeparam>
        /// <typeparam name="TW"></typeparam>
        /// <param name="nestedTrigger">The trigger that is wrapped by this purging trigger.</param>
        /// <returns></returns>
        public static PurgingWindowTrigger<TE, TW> Of<TE, TW>(WindowTrigger<TE, TW> nestedTrigger)
            where TW : Window
        {
            return new PurgingWindowTrigger<TE, TW>(nestedTrigger);
        }
    }
}
