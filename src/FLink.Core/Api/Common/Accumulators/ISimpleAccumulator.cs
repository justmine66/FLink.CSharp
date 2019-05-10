namespace FLink.Core.Api.Common.Accumulators
{
    /// <summary>
    /// Similar to Accumulator, but the type of items to add and the result value must be the same.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISimpleAccumulator<T> : IAccumulator<T, T>
    {
    }
}
