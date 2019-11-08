using FLink.Core.Util;

namespace FLink.Core.Api.Common.Functions
{
    /// <summary>
    /// Rich variant of the <see cref="IRichFunction"/>.
    /// As a <see cref="IRichFunction"/>, it gives access to the <see cref="IRuntimeContext"/> and provides setup and teardown methods: <see cref="IRichFunction.Open"/> and <see cref="IRichFunction.Close"/>
    /// </summary>
    /// <typeparam name="TInput">Type of the input elements.</typeparam>
    /// <typeparam name="TOutput">Type of the returned elements.</typeparam>
    public abstract class RichFlatMapFunction<TInput, TOutput> : AbstractRichFunction, IFlatMapFunction<TInput, TOutput>
    {
        public abstract void FlatMap(TInput value, ICollector<TOutput> output);
    }
}
