using FLink.Runtime.State;

namespace FLink.Streaming.Api.Checkpoint
{
    /// <summary>
    /// This is the core interface for <i>stateful transformation functions</i>, meaning functions that maintain state across individual stream records.
    /// While more lightweight interfaces exist as shortcuts for various types of state, this interface offer the greatest flexibility in managing both <i>keyed state</i> and <i>operator state</i>.
    /// </summary>
    public interface ICheckpointedFunction
    {
        /// <summary>
        /// This method is called when a snapshot for a checkpoint is requested. This acts as a hook to the function to ensure that all state is exposed by means previously offered through <see cref="IFunctionInitializationContext"/> when the Function was initialized, or offered now by <see cref="IFunctionSnapshotContext"/> itself.
        /// </summary>
        /// <param name="context"></param>
        void InitializeState(IFunctionInitializationContext context);

        /// <summary>
        /// This method is called when the parallel function instance is created during distributed execution. Functions typically set up their state storing data structures in this method.
        /// </summary>
        /// <param name="context">the context for initializing the operator</param>
        /// <exception cref="System.Exception"></exception>
        void SnapshotState(IFunctionSnapshotContext context);
    }
}
