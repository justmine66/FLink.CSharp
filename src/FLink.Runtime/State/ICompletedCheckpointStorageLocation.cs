namespace FLink.Runtime.State
{
    /// <summary>
    /// The CompletedCheckpointStorageLocation describes the storage aspect of a completed checkpoint.
    /// It can be used to obtain access to the metadata, get a reference pointer to the checkpoint, or to dispose the storage location.
    /// </summary>
    public interface ICompletedCheckpointStorageLocation
    {
        /// <summary>
        /// Gets the external pointer to the checkpoint.
        /// The pointer can be used to resume a program from the savepoint or checkpoint, and is typically passed as a command line argument, an HTTP request parameter, or stored in a system like ZooKeeper.
        /// </summary>
        string GetExternalPointer();

        /// <summary>
        /// Gets the state handle to the checkpoint's metadata.
        /// </summary>
        IStreamStateHandle GetMetadataHandle();

        /// <summary>
        /// Disposes the storage location.
        /// This method should be called after all state objects have been released. It typically disposes the base structure of the checkpoint storage, like the checkpoint directory.
        /// </summary>
        void DisposeStorageLocation();
    }
}
