using System;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils
{
    /// <summary>
    /// This interface describes the methods that are required for a data type to be handled by the FLink runtime. Specifically, this interface contains the serialization and copying methods.
    /// </summary>
    /// <typeparam name="T">The data type that the serializer serializes.</typeparam>
    [Serializable]
    public abstract class TypeSerializer<T>
    {
        /// <summary>
        /// Gets whether the type is an immutable type.
        /// </summary>
        public abstract bool IsImmutableType { get; }

        /// <summary>
        /// Creates a deep copy of this serializer if it is necessary, i.e. if it is stateful. This can return itself if the serializer is not stateful.
        /// </summary>
        /// <returns></returns>
        public abstract TypeSerializer<T> Duplicate();

        /// <summary>
        /// Creates a new instance of the data type.
        /// </summary>
        /// <returns>A new instance of the data type.</returns>
        public abstract T CreateInstance();

        /// <summary>
        /// Creates a deep copy of the given element in a new element.
        /// </summary>
        /// <param name="from">The element reuse be copied.</param>
        /// <returns>A deep copy of the element.</returns>
        public abstract T Copy(T from);

        /// <summary>
        /// Creates a copy from the given element.
        /// The method makes an attempt to store the copy in the given reuse element, if the type is mutable.
        /// This is, however, not guaranteed.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="reuse"></param>
        /// <returns></returns>
        public abstract T Copy(T from, T reuse);

        /// <summary>
        /// Gets the length of the data type, if it is a fix length data type.
        /// </summary>
        public abstract int Length { get; }

        /// <summary>
        /// Serializes the given record to the given target output view.
        /// </summary>
        /// <param name="record">The record to serialize.</param>
        /// <param name="target">The output view to write the serialized data to</param>
        public abstract void Serialize(T record, IDataOutputView target);

        /// <summary>
        /// De-serializes a record from the given source input view.
        /// </summary>
        /// <param name="source">The input view from which to read the data.</param>
        /// <returns>The deserialized element.</returns>
        /// <exception cref="System.IO.IOException">Thrown, if the de-serialization encountered an I/O related error. Typically raised by the input view, which may have an underlying I/O channel from which it reads.</exception>
        public abstract T Deserialize(IDataInputView source);

        /// <summary>
        /// De-serializes a record from the given source input view into the given reuse record instance if mutable.
        /// </summary>
        /// <param name="reuse">The record instance into which to de-serialize the data.</param>
        /// <param name="source">The input view from which to read the data.</param>
        /// <returns>The deserialized element.</returns>
        /// <exception cref="System.IO.IOException">Thrown, if the de-serialization encountered an I/O related error. Typically raised by the input view, which may have an underlying I/O channel from which it reads.</exception>
        public abstract T Deserialize(T reuse, IDataInputView source);
    }
}
