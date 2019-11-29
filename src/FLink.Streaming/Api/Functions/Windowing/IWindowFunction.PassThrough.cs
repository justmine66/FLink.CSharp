using System.Collections.Generic;
using FLink.Core.Util;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Functions.Windowing
{
    /// <summary>
    /// A <see cref="IWindowFunction{TInput, TOutput, TKey, TWindow}"/> that just emits each input element.
    /// </summary>
    public class PassThroughWindowFunction<TKey, TWindow, TElement> : IWindowFunction<TElement, TElement, TKey, TWindow>
        where TWindow : Window
    {
        public void Apply(TKey key, TWindow window, IEnumerable<TElement> elements, ICollector<TElement> output)
        {
            foreach (var element in elements)
                output.Collect(element);
        }
    }
}
