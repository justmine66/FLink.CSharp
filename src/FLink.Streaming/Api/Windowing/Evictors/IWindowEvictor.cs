using System.Collections.Generic;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;
using FLink.Streaming.Runtime.Operators.Windowing;

namespace FLink.Streaming.Api.Windowing.Evictors
{
    /// <summary>
    /// An evictor can remove elements from a pane before/after the evaluation of WindowFunction and after the window evaluation gets triggered by a <see cref="WindowTrigger{T,TW}"/>.
    /// </summary>
    /// <typeparam name="TElement">The type of elements that this evictor can evict.</typeparam>
    /// <typeparam name="TWindow">The type of window on which this evictor can operate.</typeparam>
    public interface IWindowEvictor<TElement, in TWindow> where TWindow : Window
    {
        /// <summary>
        /// Optionally evicts elements. Called before windowing function.
        /// </summary>
        /// <param name="elements">The elements currently in the window pane.</param>
        /// <param name="size">The current number of elements in the window pane.</param>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <param name="ctx">The context for the Evictor.</param>
        void EvictBefore(IList<TimestampedValue<TElement>> elements, int size, TWindow window, IWindowEvictorContext ctx);

        /// <summary>
        /// Optionally evicts elements. Called after windowing function.
        /// </summary>
        /// <param name="elements">The elements currently in the window pane.</param>
        /// <param name="size">The current number of elements in the window pane.</param>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <param name="ctx">The context for the Evictor.</param>
        void EvictAfter(IList<TimestampedValue<TElement>> elements, int size, TWindow window, IWindowEvictorContext ctx);
    }

    /// <summary>
    ///  A context object that is given to evictor methods.
    /// </summary>
    public interface IWindowEvictorContext
    {
        /// <summary>
        /// The current processing time.
        /// </summary>
        long CurrentProcessingTime { get; }

        /// <summary>
        /// The current watermark time.
        /// </summary>
        long CurrentWatermark { get; }
    }
}
