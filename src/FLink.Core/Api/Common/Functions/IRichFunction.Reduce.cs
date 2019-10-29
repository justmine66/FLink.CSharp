namespace FLink.Core.Api.Common.Functions
{
    public abstract class RichReduceFunction<TElement> : AbstractRichFunction, IReduceFunction<TElement>
    {
        public abstract TElement Reduce(TElement value1, TElement value2);
    }
}
