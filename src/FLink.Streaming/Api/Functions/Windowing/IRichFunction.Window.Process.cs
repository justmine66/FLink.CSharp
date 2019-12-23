using System.Collections.Generic;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.States;
using FLink.Core.Util;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Functions.Windowing
{
    /// <summary>
    /// Base abstract class for functions that are evaluated over keyed (grouped) windows using a context for retrieving extra information.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value.</typeparam>
    /// <typeparam name="TOutput">The type of the output value.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TWindow">The type of window that this window function can be applied on.</typeparam>
    public abstract class ProcessWindowFunction<TInput, TOutput, TKey, TWindow> : AbstractRichFunction
        where TWindow : Window
    {
        /// <summary>
        /// Evaluates the window and outputs none or several elements.
        /// </summary>
        /// <param name="key">The key for which this window is evaluated.</param>
        /// <param name="context">The context in which the window is being evaluated.</param>
        /// <param name="elements">The elements in the window being evaluated.</param>
        /// <param name="output">A collector for emitting elements.</param>
        /// <exception cref="System.Exception">The function may throw exceptions to fail the program and trigger recovery.</exception>
        public abstract void Process(TKey key, Context context, IEnumerable<TInput> elements, ICollector<TOutput> output);

        /// <summary>
        /// Deletes any state in the context when the Window is purged.
        /// </summary>
        /// <param name="context">The context to which the window is being evaluated.</param>
        /// <exception cref="System.Exception">The function may throw exceptions to fail the program and trigger recovery.</exception>
        public abstract void Clear(Context context);

        /// <summary>
        /// The context holding window metadata.
        /// </summary>
        public abstract class Context
        {
            /// <summary>
            /// The window that is being evaluated.
            /// </summary>
            public abstract TWindow Window { get; }

            /// <summary>
            /// The current processing time.
            /// </summary>
            public abstract long CurrentProcessingTime { get; }

            /// <summary>
            /// The current event-time watermark.
            /// </summary>
            public abstract long CurrentWatermark { get; }

            /// <summary>
            /// State accessor for per-key and per-window state.
            /// If you use per-window state you have to ensure that you clean it up by implementing <see cref="ProcessWindowFunction{TInput, TOutput, TKey, TWindow}.Clear"/>.
            /// </summary>
            public abstract IKeyedStateStore WindowState { get; }

            /// <summary>
            /// State accessor for per-key global state.
            /// </summary>
            public abstract IKeyedStateStore GlobalState { get; }

            /// <summary>
            /// Emits a record to the side output identified by the <param name="outputTag"></param>.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="outputTag">The outputTag identifies the side output to emit to.</param>
            /// <param name="value">The record to emit.</param>
            public abstract void Output<T>(OutputTag<T> outputTag, T value);
        }
    }
}
