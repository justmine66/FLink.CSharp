using System.Collections.Generic;
using FLink.Core.Util;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Functions.Windowing
{
    /// <summary>
    /// A <see cref="IWindowFunction{TInput, TOutput, TKey, TWindow}"/> that just emits each input element.
    /// </summary>
    public class PassThroughWindowFunction<K, W, T> : IWindowFunction<T, T, K, W>
        where W : Window
    {
        public void Apply(K key, W window, IEnumerable<T> elements, ICollector<T> output)
        {
            foreach (var element in elements)
                output.Collect(element);
        }
    }
}
