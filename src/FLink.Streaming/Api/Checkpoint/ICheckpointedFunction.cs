using FLink.Runtime.State;

namespace FLink.Streaming.Api.Checkpoint
{
    public interface ICheckpointedFunction
    {
        void SnapshotState(IFunctionSnapshotContext context);

        void InitializeState(IFunctionInitializationContext context)
    }
}
