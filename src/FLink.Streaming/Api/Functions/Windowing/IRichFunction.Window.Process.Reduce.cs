using System;
using System.Collections.Generic;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Util;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Functions.Windowing
{
    /// <summary>
    /// Internal <see cref="ProcessWindowFunction{TInput,TOutput,TKey,TWindow}"/> that is used for implementing a fold on a window configuration that only allows <see cref="IAllWindowFunction{TInput,TOutput,TWindow}"/> and cannot directly execute a <see cref="IReduceFunction{TElement}"/>.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TWindow"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class ReduceApplyProcessWindowFunction<TKey, TWindow, TInput, TOutput> : ProcessWindowFunction<TInput, TOutput, TKey, TWindow> where TWindow : Window
    {
        public override void Process(TKey key, Context context, IEnumerable<TInput> elements, ICollector<TOutput> output)
        {
            throw new NotImplementedException();
        }

        public override void Clear(Context context)
        {
            throw new NotImplementedException();
        }
    }
}
