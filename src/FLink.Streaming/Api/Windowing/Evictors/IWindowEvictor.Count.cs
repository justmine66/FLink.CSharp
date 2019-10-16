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
        private readonly long _maxCount;
        private readonly bool _doEvictAfter;

        public CountWindowEvictor(long maxCount)
        {
            _maxCount = maxCount;
            _doEvictAfter = false;
        }

        public CountWindowEvictor(long maxCount, bool doEvictAfter)
        {
            _maxCount = maxCount;
            _doEvictAfter = doEvictAfter;
        }

        public void EvictAfter(IList<TimestampedValue<TElement>> elements, int size, TWindow window, IWindowEvictorContext ctx)
        {
            if (!_doEvictAfter) return;

            Evict(elements, size, ctx);
        }

        public void EvictBefore(IList<TimestampedValue<TElement>> elements, int size, TWindow window, IWindowEvictorContext ctx)
        {
            if (_doEvictAfter) return;

            Evict(elements, size, ctx);
        }

        private void Evict(IList<TimestampedValue<TElement>> elements, int size, IWindowEvictorContext ctx)
        {
            if (size <= _maxCount) return;

            var evictedCount = 0;
            foreach (var element in elements)
            {
                evictedCount++;
                if (evictedCount > size - _maxCount) break;
                else elements.Remove(element);
            }
        }

        /// <summary>
        /// Creates a <see cref="CountWindowEvictor{TElement,TWindow}"/> that keeps the given number of elements.
        /// Eviction is done before the window function.
        /// </summary>
        /// <typeparam name="TE"></typeparam>
        /// <typeparam name="TW"></typeparam>
        /// <param name="maxCount">The number of elements to keep in the pane.</param>
        public static CountWindowEvictor<TE, TW> Of<TE, TW>(long maxCount) where TW : Window => new CountWindowEvictor<TE, TW>(maxCount);

        /// <summary>
        /// Creates a <see cref="CountWindowEvictor{TElement,TWindow}"/> that keeps the given number of elements.
        /// Eviction is done before the window function.
        /// </summary>
        /// <typeparam name="TE"></typeparam>
        /// <typeparam name="TW"></typeparam>
        /// <param name="maxCount">The number of elements to keep in the pane.</param>
        /// <param name="doEvictAfter">Whether to do eviction after the window function.</param>
        public static CountWindowEvictor<TE, TW> Of<TE, TW>(long maxCount, bool doEvictAfter) where TW : Window => new CountWindowEvictor<TE, TW>(maxCount, doEvictAfter);
    }
}
