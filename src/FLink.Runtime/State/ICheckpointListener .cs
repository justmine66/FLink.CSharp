namespace FLink.Runtime.State
{
    /// <summary>
    /// This interface must be implemented by functions/operations that want to receive a commit notification once a checkpoint has been completely acknowledged by all participants.
    /// </summary>
    public interface ICheckpointListener
    {
        /// <summary>
        /// This method is called as a notification once a distributed checkpoint has been completed.
        /// </summary>
        /// <remarks>Note that any exception during this method will not cause the checkpoint to fail any more.</remarks>
        /// <param name="checkpointId">The ID of the checkpoint that has been completed.</param>
        void NotifyCheckpointComplete(long checkpointId);
    }
}
