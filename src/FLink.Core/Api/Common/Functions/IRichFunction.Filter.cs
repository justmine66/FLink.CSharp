namespace FLink.Core.Api.Common.Functions
{
    public abstract class RichFilterFunction<TElement> : AbstractRichFunction, IFilterFunction<TElement>
    {
        public abstract bool Filter(TElement value);
    }
}
