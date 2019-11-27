using System;
using System.IO;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils
{
    /// <summary>
    /// This interface describes the methods that are required for a data type to be handled by the FLink runtime. Specifically, this interface contains the serialization and copying methods.
    /// The methods in this class are not necessarily thread safe. To avoid unpredictable side effects, it is recommended to call <see cref="Duplicate()"/> method and use one serializer instance per thread.
    /// Upgrading TypeSerializers to the new TypeSerializerSnapshot model.
    /// </summary>
    /// <typeparam name="T">The data type that the serializer serializes.</typeparam>
    [Serializable]
    public abstract class TypeSerializer<T>
    {
        #region [ General information about the type and the serializer ]

        /// <summary>
        /// Gets whether the type is an immutable type.
        /// </summary>
        public abstract bool IsImmutableType { get; }

        /// <summary>
        /// Creates a deep copy of this serializer if it is necessary, i.e. if it is stateful. This can return itself if the serializer is not stateful.
        /// </summary>
        /// <returns></returns>
        public abstract TypeSerializer<T> Duplicate();

        #endregion

        #region [ Instantiation & Cloning ]

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
        /// Copies exactly one record from the source input view to the target output view. Whether this operation works on binary data or partially de-serializes the record to determine its length (such as for records of variable length) is up to the implementer. Binary copies are typically faster.
        /// </summary>
        /// <param name="source">The input view from which to read the record.</param>
        /// <param name="target">The target output view to which to write the record.</param>
        /// <exception cref="IOException">Thrown if any of the two views raises an exception.</exception>
        public abstract void Copy(IDataInputView source, IDataOutputView target);

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

        #endregion

        #region [ Serializer configuration snapshot for checkpoints/savepoints ]

        /// <summary>
        /// Snapshots the configuration of this TypeSerializer. This method is only relevant if the serializer is used to state stored in checkpoints/savepoints.
        /// The snapshot of the TypeSerializer is supposed to contain all information that affects the serialization format of the serializer.
        /// The snapshot serves two purposes:
        /// 1. Reproduce the serializer when the checkpoint/savepoint is restored.
        /// 2. Check whether the serialization format is compatible with the serializer used in the restored program.
        /// </summary>
        /// <returns></returns>
        public abstract ITypeSerializerSnapshot<T> SnapshotConfiguration();

        #endregion
    }
}
