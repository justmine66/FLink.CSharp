using System.Collections.Generic;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Assigners
{
    /// <summary>
    /// A <see cref="WindowAssigner{T,TW}"/> that can merge windows.
    /// </summary>
    /// <typeparam name="T">The type of elements that this WindowAssigner can assign windows to.</typeparam>
    /// <typeparam name="TW">The type of window that this assigner assigns.</typeparam>
    public abstract class MergingWindowAssigner<T, TW> : WindowAssigner<T, TW> where TW : Window
    {
        /// <summary>
        /// Determines which windows (if any) should be merged.
        /// </summary>
        /// <param name="windows">The window candidates.</param>
        /// <param name="callback">A callback that can be invoked to signal which windows should be merged.</param>
        public abstract void MergeWindows(IEnumerable<TW> windows, IMergeCallback<TW> callback);

        public interface IMergeCallback<in TWindow>
        {
            /// <summary>
            /// Specifies that the given windows should be merged into the result window.
            /// </summary>
            /// <param name="toBeMerged">The list of windows that should be merged into one window.</param>
            /// <param name="mergeResult">The resulting merged window.</param>
            void Merge(IEnumerable<TWindow> toBeMerged, TWindow mergeResult);
        }
    }
}
