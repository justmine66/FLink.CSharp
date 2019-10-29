namespace FLink.Core.Api.Common.Functions
{
    /// <summary>
    /// A filter function is a predicate applied individually to each record. The predicate decides whether to keep the element, or to discard it.
    /// </summary>
    /// <typeparam name="TElement">The type of the filtered elements.</typeparam>
    public interface IFilterFunction<in TElement> : IFunction
    {
        /// <summary>
        /// The filter function that evaluates the predicate.
        /// </summary>
        /// <param name="value">The value to be filtered.</param>
        /// <returns>True for values that should be retained, false for values to be filtered out.</returns>
        /// <exception cref="System.Exception">This method may throw exceptions. Throwing an exception will cause the operation to fail and may trigger recovery.</exception>
        bool Filter(TElement value);
    }
}
