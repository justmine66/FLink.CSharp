using FLink.Core.Api.Common.Functions;

namespace FLink.Core.Api.CSharp
{
    /// <summary>
    /// The <see cref="IKeySelector{TInput,TKey}"/> allows to use deterministic objects for operations such as reduce, reduceGroup, join, coGroup, etc. If invoked multiple times on the same object, the returned key must be the same.
    /// The extractor takes an object and returns the deterministic key for that object.
    /// </summary>
    /// <typeparam name="TObject">Type of objects to extract the key from.</typeparam>
    /// <typeparam name="TKey">Type of key.</typeparam>
    public interface IKeySelector<in TObject, out TKey> : IFunction
    {
        /// <summary>
        /// User-defined function that deterministically extracts the key from an object.
        /// </summary>
        /// <param name="value">The object to get the key from.</param>
        /// <returns>The extracted key.</returns>
        TKey GetKey(TObject value);
    }
}
