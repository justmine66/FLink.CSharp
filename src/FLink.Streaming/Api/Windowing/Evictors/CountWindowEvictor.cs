using System;
using System.Collections.Generic;
using FLink.Streaming.Api.Windowing.Windows;
using FLink.Streaming.Runtime.Operators.Windowing;

namespace FLink.Streaming.Api.Windowing.Evictors
{
    /// <summary>
    /// An <see cref="IWindowEvictor{T,TW}"/> that keeps up to a certain amount of elements, Keeps up to a user-specified number of elements from the window and discards the remaining ones from the beginning of the window buffer.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <typeparam name="TWindow"></typeparam>
    public class CountWindowEvictor<TElement, TWindow> : IWindowEvictor<TElement, TWindow>
        where TWindow : Window
    {
        public void EvictAfter(IEnumerable<TimestampedValue<TElement>> elements, int size, TWindow window, IWindowEvictor<TElement, TWindow>.IEvictorContext evictorContext)
        {
            throw new NotImplementedException();
        }

        public void EvictBefore(IEnumerable<TimestampedValue<TElement>> elements, int size, TWindow window, IWindowEvictor<TElement, TWindow>.IEvictorContext evictorContext)
        {
            throw new NotImplementedException();
        }
    }
}
