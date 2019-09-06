using System;
using FLink.Runtime.Checkpoint;
using FLink.Runtime.State;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    ///  Base class for all stream operators. Operators that contain a user function should extend the class
    /// <see cref="AbstractUdfStreamOperator{TOut, TFunction}"/>instead(which is a specialized subclass of this class).
    /// </summary>
    /// <typeparam name="TOut">The output type of the operator</typeparam>
    public abstract class AbstractStreamOperator<TOut> : IStreamOperator<TOut>
    {
        public void NotifyCheckpointComplete(long checkpointId)
        {
            throw new NotImplementedException();
        }

        public void SetCurrentKey(object key)
        {
            throw new NotImplementedException();
        }

        public object GetCurrentKey()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        void IStreamOperator<TOut>.Dispose()
        {
            throw new NotImplementedException();
        }

        public void PrepareSnapshotPreBarrier(long checkpointId)
        {
            throw new NotImplementedException();
        }

        public OperatorSnapshotFutures SnapshotState(long checkpointId, long timestamp, CheckpointOptions checkpointOptions,
            ICheckpointStreamFactory storageLocation)
        {
            throw new NotImplementedException();
        }

        public void InitializeState()
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
