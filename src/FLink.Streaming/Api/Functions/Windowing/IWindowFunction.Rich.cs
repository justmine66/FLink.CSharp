using System.Collections.Generic;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Util;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Functions.Windowing
{
    /// <summary>
    /// Rich variant of the <see cref="IWindowFunction{TInput,TOutput,TKey,TWindow}"/>.
    /// As a <see cref="IRichFunction"/>, it gives access to the <see cref="IRuntimeContext"/> and provides setup(<see cref="IRichFunction.Open"/>) and tear-down(<see cref="IRichFunction.Close"/>) methods.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value.</typeparam>
    /// <typeparam name="TOutput">The type of the output value.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TWindow">The type of <see cref="Window"/> that this window function can be applied on.</typeparam>
    public abstract class RichWindowFunction<TInput, TOutput, TKey, TWindow> :
        AbstractRichFunction,
        IWindowFunction<TInput, TOutput, TKey, TWindow>
        where TWindow : Window
    { 
        public abstract void Apply(TKey key, TWindow window, IEnumerable<TInput> elements, ICollector<TOutput> output);
    }
}
