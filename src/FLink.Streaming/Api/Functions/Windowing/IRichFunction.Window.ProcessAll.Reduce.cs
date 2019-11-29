using System.Collections.Generic;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Util;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Functions.Windowing
{
    /// <summary>
    /// Internal <see cref="ProcessAllWindowFunction{TInput,TOutput,TWindow}"/> that is used for implementing a fold on a window configuration that only allows <see cref="IAllWindowFunction{TInput,TOutput,TWindow}"/> and cannot directly execute a <see cref="IReduceFunction{TElement}"/>.
    /// </summary>
    /// <typeparam name="TWindow"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class ReduceApplyProcessAllWindowFunction<TWindow, TInput, TOutput> : ProcessAllWindowFunction<TInput, TOutput, TWindow> where TWindow : Window
    {
        public override void Process(Context context, IEnumerable<TInput> elements, ICollector<TOutput> output)
        {
            throw new System.NotImplementedException();
        }
    }
}
