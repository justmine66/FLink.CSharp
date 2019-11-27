using FLink.Core.Memory;

namespace FLink.Core.Types
{
    /// <summary>
    /// Interface to be implemented by basic types that support to be copied efficiently.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICopyableValue<T> : IValue
    {
        /// <summary>
        /// Gets the length of the data type when it is serialized, in bytes.
        /// </summary>
        int BinaryLength { get; }

        /// <summary>
        /// Performs a deep copy of this object into the <param name="target"></param> instance.
        /// </summary>
        /// <param name="target">Object to copy into.</param>
        void CopyTo(T target);

        /// <summary>
        /// Performs a deep copy of this object into a new instance.
        /// </summary>
        /// <returns>This method is useful for generic user-defined functions to clone a <see cref="ICopyableValue{T}"/> when storing multiple objects. With object reuse a deep copy must be created and type erasure prevents calling new.</returns>
        T Copy();

        /// <summary>
        /// Copies the next serialized instance from <param name="source"></param> to <param name="target"></param>.
        /// </summary>
        /// <param name="source">Data source for serialized instance.</param>
        /// <param name="target">Data target for serialized instance.</param>
        void Copy(IDataInputView source, IDataOutputView target);
    }
}
