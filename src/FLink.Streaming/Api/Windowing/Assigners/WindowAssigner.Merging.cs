using System.Collections.Generic;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Assigners
{
    /// <summary>
    /// A <see cref="WindowAssigner{T,TW}"/> that can merge windows.
    /// </summary>
    /// <typeparam name="TElement">The type of elements that this WindowAssigner can assign windows to.</typeparam>
    /// <typeparam name="TWindow">The type of window that this assigner assigns.</typeparam>
    public abstract class MergingWindowAssigner<TElement, TWindow> : WindowAssigner<TElement, TWindow> where TWindow : Window
    {
        /// <summary>
        /// Determines which windows (if any) should be merged.
        /// </summary>
        /// <param name="windows">The window candidates.</param>
        /// <param name="callback">A callback that can be invoked to signal which windows should be merged.</param>
        public abstract void MergeWindows(IEnumerable<TWindow> windows, IMergeWindowCallback<TWindow> callback);
    }

    /// <summary>
    /// Callback to be used in <see cref="MergingWindowAssigner{T,TW}.MergeWindows"/> for specifying which windows should be merged.
    /// </summary>
    /// <typeparam name="TWindow"></typeparam>
    public interface IMergeWindowCallback<in TWindow>
    {
        /// <summary>
        /// Specifies that the given windows should be merged into the result window.
        /// </summary>
        /// <param name="toBeMerged">The list of windows that should be merged into one window.</param>
        /// <param name="mergeResult">The resulting merged window.</param>
        void Merge(IEnumerable<TWindow> toBeMerged, TWindow mergeResult);
    }
}
