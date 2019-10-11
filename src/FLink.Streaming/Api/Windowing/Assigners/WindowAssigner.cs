using System.Collections.Generic;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Assigners
{
    /// <summary>
    /// A <see cref="WindowAssigner{TElement,TWindow}"/> assigns zero or more windows to an element.
    /// </summary>
    /// <typeparam name="TElement">The type of elements that this WindowAssigner can assign windows to.</typeparam>
    /// <typeparam name="TWindow">The type of <see cref="Window"/> that this assigner assigns.</typeparam>
    public abstract class WindowAssigner<TElement, TWindow> where TWindow : Window
    {
        /// <summary>
        /// Returns a Collection of windows that should be assigned to the element.
        /// </summary>
        /// <param name="element">The element to which windows should be assigned.</param>
        /// <param name="timestamp">The timestamp of the element.</param>
        /// <param name="context">The <see cref="WindowAssignerContext"/> in which the assigner operates.</param>
        /// <returns></returns>
        public abstract IEnumerable<TWindow> AssignWindows(TElement element, long timestamp, WindowAssignerContext context);

        /// <summary>
        /// Returns the default trigger associated with this WindowAssigner.
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public abstract Trigger<TElement, TWindow> GetDefaultTrigger(StreamExecutionEnvironment env);

        /// <summary>
        /// Returns true if elements are assigned to windows based on event time, false otherwise.
        /// </summary>
        public abstract bool IsEventTime { get; }

        /// <summary>
        /// A context provided to the <see cref="WindowAssigner{T,TW}"/> that allows it to query the current processing time.
        /// </summary>
        public abstract class WindowAssignerContext
        {
            /// <summary>
            /// Returns the current processing time.
            /// </summary>
            /// <returns></returns>
            public abstract long CurrentProcessingTime { get; }
        }
    }
}