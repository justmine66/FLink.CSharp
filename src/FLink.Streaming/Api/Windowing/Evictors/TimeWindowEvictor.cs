using System;
using System.Collections.Generic;
using FLink.Core.Api.Common.Accumulators;
using FLink.Streaming.Api.Windowing.Windows;
using FLink.Streaming.Runtime.Operators.Windowing;

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
        public long WindowSize;
        public bool DoEvictAfter;

        public TimeWindowEvictor(long windowSize)
        {
            WindowSize = windowSize;
            DoEvictAfter = false;
        }

        public TimeWindowEvictor(long windowSize, bool doEvictAfter)
        {
            WindowSize = windowSize;
            DoEvictAfter = doEvictAfter;
        }

        public void EvictAfter(IList<TimestampedValue<TElement>> elements, int size, TWindow window, IWindowEvictorContext ctx)
        {
            if (!DoEvictAfter) return;

            Evict(elements, size, ctx);
        }

        public void EvictBefore(IList<TimestampedValue<TElement>> elements, int size, TWindow window, IWindowEvictorContext ctx)
        {
            if (DoEvictAfter) return;

            Evict(elements, size, ctx);
        }

        public override string ToString() => "TimeWindowEvictor(" + WindowSize + ")";

        private void Evict(IList<TimestampedValue<TElement>> elements, int size, IWindowEvictorContext ctx)
        {
            if (!HasTimestamp(elements)) return;

            var currentTime = GetMaxTimestamp(elements);
            var evictCutoff = currentTime - WindowSize;

            foreach (var element in elements)
                if (element.Timestamp <= evictCutoff)
                    elements.Remove(element);
        }

        private bool HasTimestamp(IList<TimestampedValue<TElement>> elements)
        {
            foreach (var element in elements)
                if (element.HasTimestamp) return true;

            return false;
        }

        private long GetMaxTimestamp(IList<TimestampedValue<TElement>> elements)
        {
            var accumulator = new LongMaximum();
            foreach (var element in elements)
                accumulator.Add(element.Timestamp);

            return accumulator.GetLocalValue();
        }

        /// <summary>
        /// Creates a <see cref="TimeWindowEvictor{TElement,TWindow}"/> that keeps the given number of elements.
        /// Eviction is done before the window function.
        /// </summary>
        /// <typeparam name="TE"></typeparam>
        /// <typeparam name="TW"></typeparam>
        /// <param name="windowSize">The amount of time for which to keep elements.</param>
        public static TimeWindowEvictor<TE, TW> Of<TE, TW>(TimeSpan windowSize) where TW : Window => new TimeWindowEvictor<TE, TW>((long)windowSize.TotalMilliseconds);

        /// <summary>
        /// Creates a <see cref="TimeWindowEvictor{TElement,TWindow}"/> that keeps the given number of elements.
        /// Eviction is done before the window function.
        /// </summary>
        /// <typeparam name="TE"></typeparam>
        /// <typeparam name="TW"></typeparam>
        /// <param name="windowSize">The amount of time for which to keep elements.</param>
        /// <param name="doEvictAfter">Whether eviction is done after window function.</param>
        public static TimeWindowEvictor<TE, TW> Of<TE, TW>(TimeSpan windowSize, bool doEvictAfter) where TW : Window => new TimeWindowEvictor<TE, TW>((long)windowSize.TotalMilliseconds, doEvictAfter);
    }
}
