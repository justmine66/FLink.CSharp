namespace FLink.Runtime.State
{
    /// <summary>
    /// This interface provides a context in which operators that use managed state (i.e. state that is managed by state backends) can perform a snapshot. As snapshots of the backends themselves are taken by the system, this interface mainly provides meta information about the checkpoint.
    /// </summary>
    public interface IManagedSnapshotContext
    {
        /// <summary>
        /// Returns the ID of the checkpoint for which the snapshot is taken.
        /// The checkpoint ID is guaranteed to be strictly monotonously increasing across checkpoints.
        /// </summary>
        long CheckpointId { get; }

        /// <summary>
        /// Returns timestamp (wall clock time) when the master node triggered the checkpoint for which the state snapshot is taken.
        /// </summary>
        long CheckpointTimestamp { get; }
    }
}
