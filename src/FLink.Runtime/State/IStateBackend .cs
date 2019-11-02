using System.Collections.Generic;
using System.IO;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.FS;
using FLink.Metrics.Core;
using FLink.Runtime.Execution;
using FLink.Runtime.Query;
using FLink.Runtime.State.TTL;

namespace FLink.Runtime.State
{
    /// <summary>
    /// A State Backend defines how the state of a streaming application is stored and check pointed. Different State Backend store their state in different fashions, and use different data structures to hold the state of a running application.
    /// </summary>
    public interface IStateBackend
    {
        #region [ Checkpoint storage - the durable persistence of checkpoint data ]

        /// <summary>
        /// Resolves the given pointer to a checkpoint/savepoint into a checkpoint location.
        /// The location supports reading the checkpoint metadata, or disposing the checkpoint storage location.
        /// If the state backend cannot understand the format of the pointer (for example because it was created by a different state backend) this method should throw an <see cref="IOException"/>.
        /// </summary>
        /// <param name="externalPointer">The external checkpoint pointer to resolve.</param>
        /// <returns>The checkpoint location handle.</returns>
        /// <exception cref="IOException">Thrown, if the state backend does not understand the pointer, or if the pointer could not be resolved due to an I/O error.</exception>
        ICompletedCheckpointStorageLocation ResolveCheckpoint(string externalPointer);

        /// <summary>
        /// Creates a storage for checkpoints for the given job. The checkpoint storage is used to write checkpoint data and metadata.
        /// </summary>
        /// <param name="jobId">The job to store checkpoint data for.</param>
        /// <returns>A checkpoint storage for the given job.</returns>
        /// <exception cref="IOException">Thrown if the checkpoint storage cannot be initialized.</exception>
        ICheckpointStorage CreateCheckpointStorage(JobId jobId);

        #endregion

        #region [ Structure Backends ]

        /// <summary>
        /// Creates a new <see cref="AbstractKeyedStateBackend{TKey}"/> that is responsible for holding <b>keyed state</b> and checkpointing it.
        /// Keyed State is state where each value is bound to a key.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys by which the state is organized.</typeparam>
        /// <param name="env">The environment of the task.</param>
        /// <param name="jobId">The ID of the job that the task belongs to.</param>
        /// <param name="operatorIdentifier">The identifier text of the operator.</param>
        /// <param name="keySerializer">The key-serializer for the operator.</param>
        /// <param name="numberOfKeyGroups">The number of key-groups aka max parallelism.</param>
        /// <param name="keyGroupRange">Range of key-groups for which the to-be-created backend is responsible.</param>
        /// <param name="kvStateRegistry">KvStateRegistry helper for this task.</param>
        /// <param name="ttlTimeProvider">Provider for TTL logic to judge about state expiration.</param>
        /// <param name="metricGroup">The parent metric group for all state backend metrics.</param>
        /// <param name="stateHandles">The state handles for restore.</param>
        /// <param name="cancelStreamRegistry">The registry to which created closeable objects will be registered during restore.</param>
        /// <returns>The Keyed State Backend for the given job, operator, and key group range.</returns>
        /// <exception cref="System.Exception">This method may forward all exceptions that occur while instantiating the backend.</exception>
        AbstractKeyedStateBackend<TKey> CreateKeyedStateBackend<TKey>(
            IEnvironment env,
            JobId jobId,
            string operatorIdentifier,
            TypeSerializer<TKey> keySerializer,
            int numberOfKeyGroups,
            KeyGroupRange keyGroupRange,
            TaskKvStateRegistry kvStateRegistry,
            ITtlTimeProvider ttlTimeProvider,
            IMetricGroup metricGroup,
            IList<IKeyedStateHandle> stateHandles,
            CloseableRegistry cancelStreamRegistry);

        /// <summary>
        /// Creates a new <see cref="IOperatorStateBackend"/> that can be used for storing operator state.
        /// Operator state is state that is associated with parallel operator (or function) instances, rather than with keys.
        /// </summary>
        /// <param name="env">The runtime environment of the executing task.</param>
        /// <param name="operatorIdentifier">The identifier of the operator whose state should be stored.</param>
        /// <param name="stateHandles">The state handles for restore.</param>
        /// <param name="cancelStreamRegistry">The registry to register streams to close if task canceled.</param>
        /// <returns>The OperatorStateBackend for operator identified by the job and operator identifier.</returns>
        /// <exception cref="System.Exception">This method may forward all exceptions that occur while instantiating the backend.</exception>
        IOperatorStateBackend CreateOperatorStateBackend(
            IEnvironment env,
            string operatorIdentifier,
            IList<IOperatorStateHandle> stateHandles,
            CloseableRegistry cancelStreamRegistry);

        #endregion
    }
}
