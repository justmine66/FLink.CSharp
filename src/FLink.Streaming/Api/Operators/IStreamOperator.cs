using System;
using FLink.Runtime.Checkpoint;
using FLink.Runtime.State;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// Basic interface for stream operators. Implementers would implement one of <see cref="Operators.IOneInputStreamOperator{TInput, TOutput}"/> or <see cref="Operators.TwoInputStreamOperator{TInput1, TInput2, TOutput}"/> to create operators that process elements.
    /// </summary>
    /// <remarks>
    /// The class <see cref="AbstractStreamOperator{TOutput}"/> offers default implementation for the lifecycle and properties methods.
    /// Methods of <see cref="IStreamOperator{TOutput}"/> are guaranteed not to be called concurrently. Also, if using the timer service, timer callbacks are also guaranteed not to be called concurrently with methods on <see cref="IStreamOperator{TOutput}"/>.
    /// </remarks>
    /// <typeparam name="TOutput">The output type of the operator</typeparam>
    public interface IStreamOperator<TOutput> : ICheckpointListener, IKeyContext, IDisposable
    {
        /// <summary>
        /// This method is called immediately before any elements are processed, it should contain the operator's initialization logic.
        /// </summary>
        void Open();

        /// <summary>
        /// This method is called after all records have been added to the operators via the methods <see cref="IOneInputStreamOperator{TInput, TOutput}.ProcessElement(StreamRecord{TInput})"/> or <see cref="TwoInputStreamOperator{TInput1, TInput2, TOutput}.ProcessElement1(StreamRecord{TInput1})"/> and <see cref="TwoInputStreamOperator{TInput1, TInput2, TOutput}.ProcessElement2(StreamRecord{TInput2}))"/>.
        /// </summary>
        /// <remarks>
        /// The method is expected to flush all remaining buffered data. Exceptions during this flushing of buffered should be propagated, in order to cause the operation to be recognized as failed, because the last data items are not processed properly.
        /// </remarks>
        void Close();

        /// <summary>
        /// This method is called at the very end of the operator's life, both in the case of a successful completion of the operation, and in the case of a failure and canceling.
        /// </summary>
        new void Dispose();

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
    }
}
