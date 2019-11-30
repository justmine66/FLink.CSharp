using System;
using FLink.Metrics.Core;
using FLink.Runtime.Checkpoint;
using FLink.Runtime.JobGraphs;
using FLink.Runtime.State;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// Basic interface for stream operators. Implementers would implement one of <see cref="IOneInputStreamOperator{TInput, TOutput}"/> or <see cref="ITwoInputStreamOperator{TInput1,TInput2,TOutput}"/> to create operators that process elements.
    /// </summary>
    /// <remarks>
    /// The class <see cref="AbstractStreamOperator{TOutput}"/> offers default implementation for the lifecycle and properties methods.
    /// Methods of <see cref="IStreamOperator{TOutput}"/> are guaranteed not to be called concurrently. Also, if using the timer service, timer callbacks are also guaranteed not to be called concurrently with methods on <see cref="IStreamOperator{TOutput}"/>.
    /// </remarks>
    /// <typeparam name="TOutput">The output type of the operator</typeparam>
    public interface IStreamOperator<TOutput> : ICheckpointListener, IKeyContext, IDisposable
    {
        #region [ life cycle ]

        /// <summary>
        /// This method is called immediately before any elements are processed, it should contain the operator's initialization logic.
        /// In case of recovery, this method needs to ensure that all recovered data is processed before passing back control, so that the order of elements is ensured during the recovery of an operator chain (operators are opened from the tail operator to the head operator).
        /// </summary>
        /// <exception cref="Exception">An exception in this method causes the operator to fail.</exception>
        void Open();

        /// <summary>
        /// This method is called after all records have been added to the operators via the methods
        /// <see cref="IOneInputStreamOperator{TInput, TOutput}.ProcessElement(StreamRecord{TInput})"/> or
        /// <see cref="ITwoInputStreamOperator{TInput1,TInput2,TOutput}.ProcessElement1(StreamRecord{TInput1})"/> and
        /// <see cref="ITwoInputStreamOperator{TInput1,TInput2,TOutput}.ProcessElement2(StreamRecord{TInput2})"/>.
        /// </summary>
        /// <remarks>
        /// The method is expected to flush all remaining buffered data. Exceptions during this flushing of buffered should be propagated, in order to cause the operation to be recognized as failed, because the last data items are not processed properly.
        /// </remarks>
        /// <exception cref="Exception">An exception in this method causes the operator to fail.</exception>
        void Close();

        /// <summary>
        /// This method is called at the very end of the operator's life, both in the case of a successful completion of the operation, and in the case of a failure and canceling.
        /// This method is expected to make a thorough effort to release all resources that the operator has acquired.
        /// </summary>
        new void Dispose();

        #endregion

        #region [ state snapshots ]

        /// <summary>
        /// This method is called when the operator should do a snapshot, before it emits its own checkpoint barrier.
        /// Important: This method should not be used for any actual state snapshot logic, because it will inherently be within the synchronous part of the operator's checkpoint. If heavy work is done within this method, it will affect latency and downstream checkpoint alignments.
        /// </summary>
        /// <remarks>
        /// This method is intended not for any actual state persistence, but only for emitting some data before emitting the checkpoint barrier. Operators that maintain some small transient state that is inefficient to checkpoint (especially when it would need to be checkpointed in a re-scalable way) but can simply be sent downstream before the checkpoint. An example are opportunistic pre-aggregation operators, which have small the pre-aggregation state that is frequently flushed downstream.
        /// </remarks>
        /// <param name="checkpointId">The ID of the checkpoint.</param>
        void PrepareSnapshotPreBarrier(long checkpointId);

        /// <summary>
        /// Called to draw a state snapshot from the operator.
        /// </summary>
        /// <param name="checkpointId">The ID of the checkpoint.</param>
        /// <param name="timestamp"></param>
        /// <param name="checkpointOptions"></param>
        /// <param name="storageLocation"></param>
        /// <returns>a runnable future to the state handle that points to the snapshotted state. For synchronous implementations, the runnable might already be finished.</returns>
        OperatorSnapshotFutures SnapshotState(
            long checkpointId,
            long timestamp,
            CheckpointOptions checkpointOptions,
            ICheckpointStreamFactory storageLocation);

        /// <summary>
        /// Provides a context to initialize all state in the operator.
        /// </summary>
        void InitializeState();

        #endregion

        #region [ miscellaneous ]

        void SetKeyContextElement1<T>(StreamRecord<T> record);

        void SetKeyContextElement2<T>(StreamRecord<T> record);

        ChainingStrategy ChainingStrategy { get; set; }

        IMetricGroup MetricGroup { get; }

        OperatorId OperatorId { get; }

        #endregion
    }
}
