using FLink.Core.Api.Common.State;
using FLink.Extensions.Time;
using FLink.Runtime.State;
using FLink.Streaming.Api.Checkpoint;
using System;
using System.Collections.Generic;
using FLink.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FLink.Streaming.Api.Functions.Sink
{
    /// <summary>
    /// This is a recommended base class for all of the <see cref="ISinkFunction{TInput}"/> that intend to implement exactly-once semantic.
    /// It does that by implementing two phase commit algorithm on top of the <see cref="ICheckpointedFunction"/> and <see cref="ICheckpointListener"/>. User should provide custom <see cref="TTransaction"/> (transaction handle) and implement abstract methods handling this transaction handle.
    /// </summary>
    /// <typeparam name="TInput">Input type for <see cref="ISinkFunction{TInput}"/>.</typeparam>
    /// <typeparam name="TTransaction">Transaction to store all of the information required to handle a transaction.</typeparam>
    /// <typeparam name="TContext">Context that will be shared across all invocations for the given <see cref="TwoPhaseCommitSinkFunction{IN, TXN, CONTEXT}"/> instance. Context is created once</typeparam>
    public abstract class TwoPhaseCommitSinkFunction<TInput, TTransaction, TContext> : RichSinkFunction<TInput>, ICheckpointedFunction, ICheckpointListener
    {
        private static readonly ILogger Logger = ServiceLocator.GetService<ILogger<TwoPhaseCommitSinkFunction<TInput, TTransaction, TContext>>>();

        private TransactionHolder<TTransaction> currentTransactionHolder;
        // Specifies the maximum time a transaction should remain open.
        private long transactionTimeout = long.MaxValue;

        protected IListState<IState> State { get; }

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

        protected abstract void Invoke(TTransaction transaction, TInput value, TContext context);

        /// <summary>
        /// Starts a new transaction.
        /// </summary>
        /// <returns>newly created transaction.</returns>
        protected abstract TTransaction BeginTransaction();

        /// <summary>
        /// Pre commit previously created transaction. 
        /// Pre commit must make all of the necessary steps to prepare the transaction for a commit that might happen in the future. After this point the transaction might still be aborted, but underlying implementation must ensure that commit calls on already pre committed transactions will always succeed.
        /// Usually implementation involves flushing the data.
        /// </summary>
        /// <param name="transaction"></param>
        protected abstract void PreCommit(TTransaction transaction);

        /// <summary>
        /// Commit a pre-committed transaction. If this method fail, Flink application will be restarted and <see cref="RecoverAndCommit(TTransaction)"/> will be called again for the
        /// </summary>
        /// <param name="transaction"></param>
        protected abstract void Commit(TTransaction transaction);

        /// <summary>
        /// Invoked on recovered transactions after a failure. User implementation must ensure that this call will eventually succeed. If it fails, Flink application will be restarted and it will be invoked again. If it does not succeed eventually, a data loss will occur. Transactions will be recovered in an order in which they were created.
        /// </summary>
        /// <param name="transaction"></param>
        protected void RecoverAndCommit(TTransaction transaction) => Commit(transaction);

        /// <summary>
        /// Abort a transaction.
        /// </summary>
        /// <param name="transaction"></param>
        protected abstract void Abort(TTransaction transaction);

        /// <summary>
        /// Abort a transaction that was rejected by a coordinator after a failure.
        /// </summary>
        /// <param name="transaction"></param>
        protected void RecoverAndAbort(TTransaction transaction) => Abort(transaction);
    }

    /// <summary>
    /// Adds metadata (currently only the start time of the transaction) to the transaction object.
    /// </summary>
    /// <typeparam name="TTransaction"></typeparam>
    public sealed class TransactionHolder<TTransaction> : IEquatable<TransactionHolder<TTransaction>>
    {
        private readonly TTransaction _handle;
        private readonly long _transactionStartTime;

        public TransactionHolder(TTransaction handle, long transactionStartTime)
        {
            _handle = handle;
            _transactionStartTime = transactionStartTime;
        }

        public long ElapsedTime(IClock clock) => clock.Millisecond - _transactionStartTime;

        public bool Equals(TransactionHolder<TTransaction> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return EqualityComparer<TTransaction>.Default.Equals(_handle, other._handle) && _transactionStartTime == other._transactionStartTime;
        }

        public override bool Equals(object obj) => ReferenceEquals(this, obj) || obj is TransactionHolder<TTransaction> other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<TTransaction>.Default.GetHashCode(_handle) * 397) ^ _transactionStartTime.GetHashCode();
            }
        }

        public override string ToString() => "TransactionHolder{" +
                                             "handle=" + _handle +
                                             ", transactionStartTime=" + _transactionStartTime +
                                             '}';
    }
}
