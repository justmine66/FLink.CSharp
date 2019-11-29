using System.Collections.Generic;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Util;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Functions.Windowing
{
    /// <summary>
    /// Base interface for functions that are evaluated over non-keyed windows.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value.</typeparam>
    /// <typeparam name="TOutput">The type of the output value.</typeparam>
    /// <typeparam name="TWindow">The type of <see cref="Window"/> that this window function can be applied on.</typeparam>
    public interface IAllWindowFunction<in TInput, out TOutput, in TWindow> : IFunction where TWindow : Window
    {
        /// <summary>
        /// Evaluates the window and outputs none or several elements.
        /// </summary>
        /// <param name="window">The window that is being evaluated.</param>
        /// <param name="values">The elements in the window being evaluated.</param>
        /// <param name="output">A collector for emitting elements.</param>
        void Apply(TWindow window, IEnumerable<TInput> values, ICollector<TOutput> output);
    }
}
