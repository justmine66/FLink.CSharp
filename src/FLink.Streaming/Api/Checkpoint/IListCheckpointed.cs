using System;
using System.Collections.Generic;

namespace FLink.Streaming.Api.Checkpoint
{
    /// <summary>
    /// This interface can be implemented by functions that want to store state in checkpoints.
    /// </summary>
    /// <typeparam name="T">The type of the operator state.</typeparam>
    public interface IListCheckpointed<T>
    {
        /// <summary>
        /// Gets the current state of the function. The state must reflect the result of all prior invocations to this function.
        /// </summary>
        /// <param name="checkpointId">The ID of the checkpoint - a unique and monotonously increasing value.</param>
        /// <param name="timestamp">The wall clock timestamp when the checkpoint was triggered by the master.</param>
        /// <returns>The operator state in a list of redistributable, atomic sub-states. Should not return null, but empty list instead.</returns>
        /// <exception cref="Exception">Thrown if the creation of the state object failed. This causes the checkpoint to fail. The system may decide to fail the operation (and trigger recovery), or to discard this checkpoint attempt and to continue running and to try again with the next checkpoint attempt.</exception>
        List<T> SnapshotState(long checkpointId, long timestamp);

        /// <summary>
        /// Restores the state of the function or operator to that of a previous checkpoint. This method is invoked when the function is executed after a failure recovery. he state list may be empty if no state is to be recovered by the particular parallel instance of the function.
        /// </summary>
        /// <param name="state">The state to be restored as a list of atomic sub-states.</param>
        /// <exception cref="Exception">Thrown if the creation of the state object failed. This causes the checkpoint to fail. The system may decide to fail the operation (and trigger recovery), or to discard this checkpoint attempt and to continue running and to try again with the next checkpoint attempt.</exception>
        void RestoreState(List<T> state);
    }
}
