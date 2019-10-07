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
        public virtual void NotifyCheckpointComplete(long checkpointId)
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

        /// <summary>
        /// This method is called immediately before any elements are processed, it should contain the operator's initialization logic, e.g. state initialization.
        /// </summary>
        /// <exception cref="System.Exception">An exception in this method causes the operator to fail.</exception>
        public abstract void Open();

        /// <summary>
        /// This method is called after all records have been added to the operators.
        /// The method is expected to flush all remaining buffered data. Exceptions during this flushing of buffered should be propagated, in order to cause the operation to be recognized asa failed, because the last data items are not processed properly.
        /// </summary>
        /// <exception cref="System.Exception">An exception in this method causes the operator to fail.</exception>
        public abstract void Close();

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }

        public void PrepareSnapshotPreBarrier(long checkpointId)
        {
            throw new NotImplementedException();
        }

        public virtual OperatorSnapshotFutures SnapshotState(long checkpointId, long timestamp, CheckpointOptions checkpointOptions,
            ICheckpointStreamFactory storageLocation)
        {
            throw new NotImplementedException();
        }

        public virtual void InitializeState()
        {
            throw new NotImplementedException();
        }
    }
}
