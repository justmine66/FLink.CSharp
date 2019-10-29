namespace FLink.Core.Api.Common.Functions
{
    public abstract class RichAggregateFunction<TInput, TAccumulator, TOutput> : AbstractRichFunction, IAggregateFunction<TInput, TAccumulator, TOutput>
    {
        public abstract TAccumulator Add(TInput value, TAccumulator accumulator);

        public abstract TAccumulator CreateAccumulator();

        public abstract TOutput GetResult(TAccumulator accumulator);

        public abstract TAccumulator Merge(TAccumulator a, TAccumulator b);
    }
}
