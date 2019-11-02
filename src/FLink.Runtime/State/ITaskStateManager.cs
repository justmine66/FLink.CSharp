using FLink.Runtime.Checkpoint;

namespace FLink.Runtime.State
{
    /// <summary>
    /// This interface provides methods to report and retrieve state for a task.
    /// When a checkpoint or savepoint is triggered on a task, it will create snapshots for all stream operator instances it owns. All operator snapshots from the task are then reported via this interface. A typical implementation will dispatch and forward the reported state information to interested parties such as the checkpoint coordinator or a local state store.
    /// This interface also offers the complementary method that provides access to previously saved state of operator instances in the task for restore purposes.
    /// </summary>
    public interface ITaskStateManager : ICheckpointListener
    {
        void ReportTaskStateSnapshots(
            CheckpointMetaData checkpointMetaData,
            CheckpointMetrics checkpointMetrics,
            TaskStateSnapshot acknowledgedState,
            TaskStateSnapshot localState);
    }
}
