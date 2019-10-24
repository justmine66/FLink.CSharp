using System.Collections.Generic;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Util;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Functions.Windowing
{
    /// <summary>
    /// Base interface for functions that are evaluated over keyed (grouped) windows. This is an older version of ProcessWindowFunction that provides less contextual information and does not have some advances features, such as per-window keyed state. This interface will be deprecated at some point.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value.</typeparam>
    /// <typeparam name="TOutput">The type of the output value.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TWindow">The type of <see cref="Window"/> that this window function can be applied on.</typeparam>
    public interface IWindowFunction<in TInput, out TOutput, in TKey, in TWindow> : IFunction
    where TWindow : Window
    {
        /// <summary>
        /// Evaluates the window and outputs none or several elements.
        /// </summary>
        /// <param name="key">The key for which this window is evaluated.</param>
        /// <param name="window">The window that is being evaluated.</param>
        /// <param name="elements">The elements in the window being evaluated.</param>
        /// <param name="output">A collector for emitting elements.</param>
        /// <exception cref="System.Exception">The function may throw exceptions to fail the program and trigger recovery.</exception>
        void Apply(TKey key, TWindow window, IEnumerable<TInput> elements, ICollector<TOutput> output);
    }
}
