using FLink.Core.Util;

namespace FLink.Core.Api.Common.Functions
{
    /// <summary>
    /// Base interface for flatMap functions.
    /// </summary>
    /// <remarks>
    /// FlatMap functions take elements and transform them, into zero, one, or more elements. Typical applications can be splitting elements, or unnesting lists and arrays. Operations that produce multiple strictly one result element per input element can also use the <see cref="IMapFunction"/>.
    /// </remarks>
    /// <typeparam name="TInput">Type of the input elements.</typeparam>
    /// <typeparam name="TOutput">Type of the returned elements.</typeparam>
    public interface IFlatMapFunction<in TInput, out TOutput> : IFunction
    {
        void FlatMap(TInput value, ICollector<TOutput> output);
    }
}
