using System.IO;

namespace FLink.Runtime.State
{
    /// <summary>
    /// This interface creates a <see cref="ICheckpointStorageLocation"/> to which an individual checkpoint or savepoint is stored.
    /// Methods of this interface act as an administration role in checkpoint coordinator.
    /// </summary>
    public interface ICheckpointStorageCoordinatorView
    {
        /// <summary>
        /// Checks whether this backend supports highly available storage of data.
        /// Some state backends may not support highly-available durable storage, with default settings, which makes them suitable for zero-config prototyping, but not for actual production setups.
        /// </summary>
        bool SupportsHighlyAvailableStorage();

        /// <summary>
        /// Checks whether the storage has a default savepoint location configured.
        /// </summary>
        bool HasDefaultSavepointLocation();

        /// <summary>
        /// Resolves the given pointer to a checkpoint/savepoint into a checkpoint location.
        /// The location supports reading the checkpoint metadata, or disposing the checkpoint storage location.
        /// If the state backend cannot understand the format of the pointer (for example because it was created by a different state backend) this method should throw an <see cref="IOException"/>.
        /// </summary>
        /// <param name="externalPointer">The external checkpoint pointer to resolve.</param>
        /// <returns>The checkpoint location handle.</returns>
        /// <exception cref="IOException">Thrown, if the state backend does not understand the pointer, or if the pointer could not be resolved due to an I/O error.</exception>
        ICompletedCheckpointStorageLocation ResolveCheckpoint(string externalPointer);

        /// <summary>
        /// Initializes the necessary prerequisites for storage locations of checkpoints/savepoints.
        /// For file-based checkpoint storage, this method would initialize essential base checkpoint directories on checkpoint coordinator side and should be executed before calling <see cref="InitializeLocationForCheckpoint"/> and <see cref="InitializeLocationForSavepoint"/>.
        /// </summary>
        /// <exception cref="IOException">Thrown, if these base storage locations cannot be initialized due to an I/O exception.</exception>
        void InitializeBaseLocations();

        /// <summary>
        /// Initializes a storage location for new checkpoint with the given ID.
        /// The returned storage location can be used to write the checkpoint data and metadata to and to obtain the pointers for the location(s) where the actual checkpoint data should be stored.
        /// </summary>
        /// <param name="checkpointId">The ID (logical timestamp) of the checkpoint that should be persisted.</param>
        /// <returns>A storage location for the data and metadata of the given checkpoint.</returns>
        /// <exception cref="IOException">Thrown if the storage location cannot be initialized due to an I/O exception.</exception>
        ICheckpointStorageLocation InitializeLocationForCheckpoint(long checkpointId);

        /// <summary>
        /// Initializes a storage location for new savepoint with the given ID.
        /// If an external location pointer is passed, the savepoint storage location will be initialized at the location of that pointer.
        /// If the external location pointer is null, the default savepoint location will be used.
        /// If no default savepoint location is configured, this will throw an exception. Whether a default savepoint location is configured can be checked via <see cref="HasDefaultSavepointLocation"/>.
        /// </summary>
        /// <param name="checkpointId">The ID (logical timestamp) of the savepoint's checkpoint.</param>
        /// <param name="externalLocationPointer">Optionally, a pointer to the location where the savepoint should be stored. May be null.</param>
        /// <returns>A storage location for the data and metadata of the savepoint.</returns>
        /// <exception cref="IOException">Thrown if the storage location cannot be initialized due to an I/O exception.</exception>
        ICheckpointStorageLocation InitializeLocationForSavepoint(long checkpointId, string externalLocationPointer = default);
    }
}
