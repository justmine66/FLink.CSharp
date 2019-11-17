using FLink.Runtime.State;
using FLink.Streaming.Api.Checkpoint;

namespace FLink.Streaming.Api.Functions.Sink
{
    /// <summary>
    /// This is a recommended base class for all of the <see cref="ISinkFunction{TInput}"/> that intend to implement exactly-once semantic.
    /// It does that by implementing two phase commit algorithm on top of the <see cref="ICheckpointedFunction"/> and <see cref="ICheckpointListener"/>. User should provide custom <see cref="TXN"/> (transaction handle) and implement abstract methods handling this transaction handle.
    /// </summary>
    /// <typeparam name="IN">Input type for <see cref="ISinkFunction{TInput}"/>.</typeparam>
    /// <typeparam name="TXN">Transaction to store all of the information required to handle a transaction.</typeparam>
    /// <typeparam name="CONTEXT">Context that will be shared across all invocations for the given <see cref="TwoPhaseCommitSinkFunction{IN, TXN, CONTEXT}"/> instance. Context is created once</typeparam>
public abstract class TwoPhaseCommitSinkFunction<IN, TXN, CONTEXT> : RichSinkFunction<IN>, ICheckpointedFunction, ICheckpointListener
    {
        public void InitializeState(IFunctionInitializationContext context)
        {
            throw new System.NotImplementedException();
        }

        public void NotifyCheckpointComplete(long checkpointId)
        {
            throw new System.NotImplementedException();
        }

        public void SnapshotState(IFunctionSnapshotContext context)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Starts a new transaction.
        /// </summary>
        /// <returns>newly created transaction.</returns>
        protected abstract TXN BeginTransaction();

        /// <summary>
        /// Pre commit previously created transaction. 
        /// Pre commit must make all of the necessary steps to prepare the transaction for a commit that might happen in the future. After this point the transaction might still be aborted, but underlying implementation must ensure that commit calls on already pre committed transactions will always succeed.
        /// Usually implementation involves flushing the data.
        /// </summary>
        /// <param name="transaction"></param>
        protected abstract void PreCommit(TXN transaction);

        /// <summary>
        /// Commit a pre-committed transaction. If this method fail, Flink application will be restarted and <see cref="RecoverAndCommit(TXN)"/> will be called again for the
        /// </summary>
        /// <param name="transaction"></param>
        protected abstract void Commit(TXN transaction);

        /// <summary>
        /// Invoked on recovered transactions after a failure. User implementation must ensure that this call will eventually succeed. If it fails, Flink application will be restarted and it will be invoked again. If it does not succeed eventually, a data loss will occur. Transactions will be recovered in an order in which they were created.
        /// </summary>
        /// <param name="transaction"></param>
        protected void RecoverAndCommit(TXN transaction) => Commit(transaction);

        /// <summary>
        /// Abort a transaction.
        /// </summary>
        /// <param name="transaction"></param>
        protected abstract void Abort(TXN transaction);

        /// <summary>
        /// Abort a transaction that was rejected by a coordinator after a failure.
        /// </summary>
        /// <param name="transaction"></param>
        protected void RecoverAndAbort(TXN transaction) => Abort(transaction);
    }
}
