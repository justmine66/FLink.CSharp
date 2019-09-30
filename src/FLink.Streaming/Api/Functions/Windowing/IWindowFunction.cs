using System.Collections.Generic;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Util;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Functions.Windowing
{
    /// <summary>
    /// Base interface for functions that are evaluated over keyed (grouped) windows.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value.</typeparam>
    /// <typeparam name="TOut">The type of the output value.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TW">The type of <see cref="Window"/> that this window function can be applied on.</typeparam>
    public interface IWindowFunction<in TIn, out TOut, in TKey, in TW> : IFunction
    where TW : Window
    {
        /// <summary>
        /// Evaluates the window and outputs none or several elements.
        /// </summary>
        /// <param name="key">The key for which this window is evaluated.</param>
        /// <param name="window">The window that is being evaluated.</param>
        /// <param name="input">The elements in the window being evaluated.</param>
        /// <param name="output">A collector for emitting elements.</param>
        /// <exception cref="System.Exception">The function may throw exceptions to fail the program and trigger recovery.</exception>
        void Apply(TKey key, TW window, IEnumerable<TIn> input, ICollector<TOut> output);
    }
}
