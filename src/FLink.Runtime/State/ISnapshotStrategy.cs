using System.Threading.Tasks;
using FLink.Runtime.Checkpoint;

namespace FLink.Runtime.State
{
    /// <summary>
    /// Interface for different snapshot approaches in state backends. Implementing classes should ideally be stateless or at least threadsafe, i.e. this is a functional interface and is can be called in parallel by multiple checkpoints.
    /// </summary>
    /// <typeparam name="TState">type of the returned state object that represents the result of the snapshot operation.</typeparam>
    public interface ISnapshotStrategy<TState> where TState : IStateObject
    {
        TaskCompletionSource<TState> Snapshot(long checkpointId, long timestamp, ICheckpointStreamFactory streamFactory, CheckpointOptions checkpointOptions);
    }
}
