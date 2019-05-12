namespace FLink.Runtime.State
{
    /// <summary>
    /// The scope for a chunk of checkpointed state. Defines whether state is owned by one checkpoint, or whether it is shared by multiple checkpoints.
    /// </summary>
    /// <remarks>
    /// Different checkpoint storage implementations may treat checkpointed state of different scopes differently, for example put it into different folders or tables.
    /// </remarks>
    public enum CheckpointedStateScope
    {
        /// <summary>
        /// Exclusive state belongs exclusively to one specific checkpoint / savepoint.
        /// </summary>
        Exclusive,

        /// <summary>
        /// Shared state may belong to more than one checkpoint.
        /// </summary>
        /// <remarks>
        /// Shared state is typically used for incremental or differential checkpointing methods, where only deltas are written, and state from prior checkpoints is referenced in newer checkpoints as well.
        /// </remarks>
        Shared
    }
}
