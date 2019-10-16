using FLink.Streaming.Api.Windowing.Windows;
using FLink.Streaming.Runtime.Operators.Windowing;
using System;
using System.Collections.Generic;

namespace FLink.Streaming.Api.Windowing.Evictors
{
    /// <summary>
    /// An {@link Evictor} that keeps elements for a certain amount of time. takes as argument an interval in milliseconds and for a given window, it finds the maximum timestamp max_ts among its elements and removes all the elements with timestamps smaller than max_ts - interval.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <typeparam name="TWindow"></typeparam>
    public class TimeWindowEvictor<TElement, TWindow> : IWindowEvictor<TElement, TWindow>
        where TWindow : Window
    {
        public void EvictAfter(IEnumerable<TimestampedValue<TElement>> elements, int size, TWindow window, IWindowEvictorContext ctx)
        {
            throw new NotImplementedException();
        }

        public void EvictBefore(IEnumerable<TimestampedValue<TElement>> elements, int size, TWindow window, IWindowEvictorContext ctx)
        {
            throw new NotImplementedException();
        }
    }
}
