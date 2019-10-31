using System.IO;

namespace FLink.Runtime.State
{
    /// <summary>
    /// This interface implements the durable storage of checkpoint data and metadata streams.
    /// An individual checkpoint or savepoint is stored to a <see cref="ICheckpointStorageLocation"/> which created by <see cref="ICheckpointStorageCoordinatorView"/>.
    /// Methods of this interface act as a worker role in task manager.
    /// </summary>
    public interface ICheckpointStorageWorkerView
    {
        /// <summary>
        /// Resolves a storage location reference into a CheckpointStreamFactory.
        /// </summary>
        /// <param name="checkpointId">The ID of the checkpoint that the location is initialized for.</param>
        /// <param name="reference">The checkpoint location reference.</param>
        /// <returns>A checkpoint storage location reflecting the reference and checkpoint ID.</returns>
        /// <exception cref="IOException">Thrown, if the storage location cannot be initialized from the reference.</exception>
        ICheckpointStreamFactory ResolveCheckpointStorageLocation(long checkpointId, CheckpointStorageLocationReference reference);

        /// <summary>
        /// Opens a stream to persist checkpoint state data that is owned strictly by tasks and not attached to the life cycle of a specific checkpoint.
        /// </summary>
        /// <returns>A checkpoint state stream to the location for state owned by tasks.</returns>
        /// <exception cref="IOException">Thrown, if the stream cannot be opened.</exception>
        CheckpointStateOutputStream CreateTaskOwnedStateStream();
    }
}
