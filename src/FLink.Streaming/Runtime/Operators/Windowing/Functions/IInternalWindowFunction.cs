using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.State;
using FLink.Core.Util;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Runtime.Operators.Windowing.Functions
{
    /// <summary>
    /// Internal interface for functions that are evaluated over keyed (grouped) windows.
    /// </summary>
    /// <typeparam name="TInput">The type of the input value.</typeparam>
    /// <typeparam name="TOutput">The type of the output value.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TWindow"></typeparam>
    public interface IInternalWindowFunction<in TInput, out TOutput, in TKey, in TWindow> : IFunction where TWindow : Window
    {
        /// <summary>
        /// Evaluates the window and outputs none or several elements.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="window"></param>
        /// <param name="context">The context in which the window is being evaluated.</param>
        /// <param name="input">The elements in the window being evaluated.</param>
        /// <param name="output">A collector for emitting elements.</param>
        void Process(TKey key, TWindow window, IInternalWindowContext context, TInput input, ICollector<TOutput> output);

        /// <summary>
        ///  Deletes any state in the <paramref name="context"/> when the Window is purged.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="context">The context to which the window is being evaluated</param>
        /// <exception cref="System.Exception">The function may throw exceptions to fail the program and trigger recovery.</exception>
        void Clear(TWindow window, IInternalWindowContext context);
    }

    /// <summary>
    /// A context for <see cref="IInternalWindowFunction{TIn,TOut,TKey,TW}"/>.
    /// </summary>
    public interface IInternalWindowContext
    {
        long CurrentProcessingTime { get; }

        long CurrentWatermark { get; }

        IKeyedStateStore WindowState { get; }
        IKeyedStateStore GlobalState { get; }

        void Output<T>(OutputTag<T> outputTag, T value);
    }
}
