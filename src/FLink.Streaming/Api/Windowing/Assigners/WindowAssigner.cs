using System.Collections.Generic;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Assigners
{
    /// <summary>
    /// A WindowAssigner is responsible for assigning each incoming element to one or more windows.
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
        public abstract IEnumerable<TWindow> AssignWindows(TElement element, long timestamp, WindowAssignerContext context);

        /// <summary>
        /// Returns the default trigger associated with this WindowAssigner.
        /// </summary>
        /// <param name="env"></param>
        public abstract WindowTrigger<TElement, TWindow> GetDefaultTrigger(StreamExecutionEnvironment env);

        /// <summary>
        /// Returns a <see cref="TypeSerializer{T}"/> for serializing windows that are assigned by this <see cref="WindowAssigner{TElement,TWindow}"/>.
        /// </summary>
        /// <param name="executionConfig"></param>
        public abstract TypeSerializer<TWindow> GetWindowSerializer(ExecutionConfig executionConfig);

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