namespace FLink.Core.Api.Common.Functions
{
    /// <summary>
    /// Base interface for Map functions. Map functions take elements and transform them, element wise. A Map function always produces a single result element for each input element. Typical applications are parsing elements, converting data types, or projecting out fields. Operations that produce multiple result elements from a single input element can be implemented using the <see cref="IFlatMapFunction{TInput,TOutput}"/>.
    /// </summary>
    /// <typeparam name="TInput">Type of the input elements.</typeparam>
    /// <typeparam name="TOutput">Type of the returned elements.</typeparam>
    public interface IMapFunction<in TInput, out TOutput> : IFunction
    {
        /// <summary>
        /// Takes an element from the input data set and transforms it into exactly one element.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <returns>The transformed value</returns>
        TOutput Map(TInput value);
    }
}
