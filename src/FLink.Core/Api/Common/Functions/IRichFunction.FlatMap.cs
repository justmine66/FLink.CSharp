using FLink.Core.Util;

namespace FLink.Core.Api.Common.Functions
{
    public abstract class RichFlatMapFunction<TInput, TOutput>: AbstractRichFunction, IFlatMapFunction<TInput, TOutput>
    {
        public abstract void FlatMap(TInput value, ICollector<TOutput> output);
    }
}
