using System.Collections.Generic;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.State;
using FLink.Core.Util;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Functions.Windowing
{
    /// <summary>
    /// Base abstract class for functions that are evaluated over non-keyed windows using a context for retrieving extra information.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value.</typeparam>
    /// <typeparam name="TOutput">The type of the output value.</typeparam>
    /// <typeparam name="TWindow">The type of <see cref="Window"/> that this window function can be applied on.</typeparam>
    public abstract class ProcessAllWindowFunction<TInput, TOutput, TWindow> : AbstractRichFunction where TWindow : Window
    {
        /// <summary>
        /// Evaluates the window and outputs none or several elements. 
        /// </summary>
        /// <param name="context">The context in which the window is being evaluated.</param>
        /// <param name="elements">The elements in the window being evaluated.</param>
        /// <param name="output">A collector for emitting elements.</param>
        public abstract void Process(Context context, IEnumerable<TInput> elements, ICollector<TOutput> output);

        /// <summary>
        /// The context holding window metadata.
        /// </summary>
        public abstract class Context
        {
            /// <summary>
            /// Gets the window that is being evaluated.
            /// </summary>
            public abstract TWindow Window { get; }

            /// <summary>
            /// State accessor for per-key and per-window state.
            /// NOTE: If you use per-window state you have to ensure that you clean it up by implementing <see cref="ProcessWindowFunction{TInput,TOutput,TKey,TWindow}.Clear"/>.
            /// </summary>
            /// <returns></returns>
            public abstract IKeyedStateStore WindowState();

            /// <summary>
            /// State accessor for per-key global state.
            /// </summary>
            /// <returns></returns>
            public abstract IKeyedStateStore GlobalState();

            /// <summary>
            /// Emits a record to the side output identified.
            /// </summary>
            /// <typeparam name="TRecord"></typeparam>
            /// <param name="outputTag">Identifies the side output to emit to.</param>
            /// <param name="value">value The record to emit.</param>
            public abstract void Output<TRecord>(OutputTag<TRecord> outputTag, TRecord value);
        }
    }
}
