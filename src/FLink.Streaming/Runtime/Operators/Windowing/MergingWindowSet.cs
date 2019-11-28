using System;
using System.Collections.Generic;
using FLink.Core.Api.Common.State;
using FLink.Core.Exceptions;
using FLink.Extensions.DependencyInjection;
using FLink.Streaming.Api.Operators;
using FLink.Streaming.Api.Windowing.Assigners;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;
using Microsoft.Extensions.Logging;

namespace FLink.Streaming.Runtime.Operators.Windowing
{
    /// <summary>
    /// Utility for keeping track of merging <see cref="Window"/> when using a <see cref="MergingWindowAssigner{TElement,TWindow}"/> in a <see cref="WindowOperator{TKey,TInput,TAccumulator,TOutput,TWindow}"/>.
    /// When merging windows, we keep one of the original windows as the state window, i.e. the window that is used as namespace to store the window elements. Elements from the state windows of merged windows must be merged into this one state window. We keep a mapping from in-flight window to state window that can be queried using <see cref=""/>.
    /// </summary>
    /// <typeparam name="TWindow"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    public class MergingWindowSet<TElement, TWindow> where TWindow : Window
    {
        private static readonly ILogger Logger = ServiceLocator.GetService<ILogger<MergingWindowSet<TElement, TWindow>>>();

        /// <summary>
        /// Mapping from window to the window that keeps the window state. When we are incrementally merging windows starting from some window we keep that starting window as the state window to prevent costly state juggling.
        /// </summary>
        private readonly Dictionary<TWindow, TWindow> _mapping;

        /// <summary>
        /// Mapping when we created the <see cref="MergingWindowSet{W}"/>. We use this to decide whether we need to persist any changes to state.
        /// </summary>
        private readonly Dictionary<TWindow, TWindow> _initialMapping;

        private readonly IListState<(TWindow, TWindow)> _state;

        private readonly MergingWindowAssigner<TElement, TWindow> _windowAssigner;

        /// <summary>
        /// Restores a <see cref="MergingWindowSet{TElement,W}"/> from the given state.
        /// </summary>
        /// <param name="windowAssigner"></param>
        /// <param name="state"></param>
        public MergingWindowSet(MergingWindowAssigner<TElement, TWindow> windowAssigner, IListState<(TWindow, TWindow)> state)
        {
            _windowAssigner = windowAssigner;
            _mapping = new Dictionary<TWindow, TWindow>();

            var windowState = state.Get();
            if (windowState != null)
                foreach (var (window1, window2) in windowState)
                    _mapping.Add(window1, window2);

            _state = state;
            _initialMapping = new Dictionary<TWindow, TWindow>(_mapping);
        }

        /// <summary>
        /// Persist the updated mapping to the given state if the mapping changed since initialization.
        /// </summary>
        public void Persist()
        {
            if (_mapping.Equals(_initialMapping)) return;

            _state.Clear();

            foreach (var (window1, window2) in _mapping)
            {
                _state.Add((window1, window2));
            }
        }

        /// <summary>
        /// Gets the state window for the given in-flight <see cref="Window"/>. The state window is the <see cref="Window"/> in which we keep the actual state of a given in-flight window. Windows might expand but we keep to original state window for keeping the elements of the window to avoid costly state juggling.
        /// </summary>
        /// <param name="window">The window for which to get the state window.</param>
        /// <returns></returns>
        public TWindow GetStateWindow(TWindow window) => _mapping[window];

        /// <summary>
        /// Removes the given window from the set of in-flight windows.
        /// </summary>
        /// <param name="window">The <see cref="Window"/> to remove.</param>
        public void RetireWindow(TWindow window)
        {
            var stateWindow = _mapping[window];
            if (stateWindow == null)
                throw new IllegalStateException(
                    $"Window {window} is not in in-flight window set.");

            _mapping.Remove(window);
        }

        /// <summary>
        /// Adds a new <see cref="Window"/> to the set of in-flight windows. It might happen that this triggers merging of previously in-flight windows. In that case, the provided <see cref="IMergeFunction{W}"/> is called.
        /// This returns the window that is the representative of the added window after adding. This can either be the new window itself, if no merge occurred, or the newly merged window. Adding an element to a window or calling trigger functions should only happen on the returned representative. This way, we never have to deal with a new window that is immediately swallowed up by another window.
        /// </summary>
        /// <param name="newWindow">The new <see cref="Window"/> to add.</param>
        /// <param name="mergeFunction">The callback to be invoked in case a merge occurs.</param>
        /// <returns>The window that new new window ended up in. This can also be the the new window itself in case no merge occurred.</returns>
        public TWindow AddWindow(TWindow newWindow, IMergeFunction<TWindow> mergeFunction)
        {
            var windows = new List<TWindow>();

            windows.AddRange(_mapping.Values);
            windows.Add(newWindow);

            var mergeResults = new Dictionary<TWindow, IList<TWindow>>();
            _windowAssigner.MergeWindows(windows, new MergeWindowCallback<TWindow>(mergeResults, Logger));

            var resultWindow = newWindow;
            var mergedNewWindow = false;

            foreach (var (mergeResult, mergedWindows) in mergeResults)
            {
                // if our new window is in the merged windows make the merge result the result window
                if (mergedWindows.Remove(newWindow))
                {
                    mergedNewWindow = true;
                    resultWindow = mergeResult;
                }

                // pick any of the merged windows and choose that window's state window as the state window for the merge result
                var mergedStateWindow = _mapping[mergedWindows.GetEnumerator().Current];

                // figure out the state windows that we are merging
                var mergedStateWindows = new List<TWindow>();
                foreach (var mergedWindow in mergedWindows)
                {
                    _mapping.Remove(mergedWindow);
                    mergedStateWindows.Add(mergedWindow);
                }

                _mapping.Add(mergeResult, mergedStateWindow);

                // don't put the target state window into the merged windows
                mergedStateWindows.Remove(mergedStateWindow);

                // don't merge the new window itself, it never had any state associated with it
                // i.e. if we are only merging one pre-existing window into itself
                // without extending the pre-existing window
                if (!(mergedWindows.Contains(mergeResult) && mergedWindows.Count == 1))
                {
                    mergeFunction.Merge(mergeResult,
                        mergedWindows,
                        _mapping[mergeResult],
                        mergedStateWindows);
                }
            }

            // the new window created a new, self-contained window without merging
            if (mergeResults.Count <= 0 || (resultWindow.Equals(newWindow) && !mergedNewWindow))
                _mapping.Add(resultWindow, resultWindow);

            return resultWindow;
        }

        public interface IMergeFunction<TW>
        {
            /// <summary>
            /// This gets called when a merge occurs.
            /// </summary>
            /// <param name="mergeResult">The newly resulting merged window.</param>
            /// <param name="mergedWindows">The merged windows.</param>
            /// <param name="stateWindowResult">The state window of the merge result.</param>
            /// <param name="mergedStateWindows">The merged state windows.</param>
            void Merge(TW mergeResult, IList<TW> mergedWindows, TW stateWindowResult,
                IList<TW> mergedStateWindows);
        }
    }
}
