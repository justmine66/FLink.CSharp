namespace FLink.Core.Api.Common.Functions
{
    public abstract class RichMapFunction<TInput, TOutput>: AbstractRichFunction, IMapFunction<TInput, TOutput>
    {
        public abstract TOutput Map(TInput value);
    }
}
