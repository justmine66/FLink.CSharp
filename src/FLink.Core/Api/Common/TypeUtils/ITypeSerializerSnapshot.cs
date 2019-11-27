using FLink.Core.Memory;
using System.IO;

namespace FLink.Core.Api.Common.TypeUtils
{
    /// <summary>
    /// A <see cref="ITypeSerializerSnapshot{T}"/> is a point-in-time view of a <see cref="TypeSerializer{T}"/>'s configuration. The configuration snapshot of a serializer is persisted within checkpoints as a single source of meta information about the schema of serialized data in the checkpoint.
    /// </summary>
    /// <typeparam name="T">The data type that the originating serializer of this configuration serializes.</typeparam>
    public interface ITypeSerializerSnapshot<T>
    {
        /// <summary>
        /// Gets the version of the current snapshot's written binary format.
        /// </summary>
        int CurrentVersion { get; }

        /// <summary>
        /// Writes the serializer snapshot to the provided <see cref="IDataOutputView"/>.
        /// The current version of the written serializer snapshot's binary format is specified by the <see cref="CurrentVersion"/>.
        /// </summary>
        /// <param name="output">the <see cref="IDataOutputView"/> to write the snapshot to.</param>
        /// <exception cref="IOException">Thrown if the snapshot data could not be written.</exception>
        void WriteSnapshot(IDataOutputView output);

        /// <summary>
        /// Reads the serializer snapshot from the provided <see cref="IDataInputView"/>.
        /// The version of the binary format that the serializer snapshot was written with is provided. This version can be used to determine how the serializer snapshot should be read.
        /// </summary>
        /// <param name="readVersion">version of the serializer snapshot's written binary format</param>
        /// <param name="input">in the <see cref="IDataInputView"/> to read the snapshot from.</param>
        void ReadSnapshot(int readVersion, IDataInputView input);

        /// <summary>
        /// Recreates a serializer instance from this snapshot. The returned serializer can be safely used to read data written by the prior serializer(i.e., the serializer that created this snapshot).
        /// </summary>
        /// <returns>instance restored from this serializer snapshot.</returns>
        TypeSerializer<T> RestoreSerializer();
    }
}
