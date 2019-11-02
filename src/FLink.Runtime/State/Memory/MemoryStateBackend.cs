using System.Collections.Generic;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Configurations;
using FLink.Core.FS;
using FLink.Core.Util;
using FLink.Metrics.Core;
using FLink.Runtime.Execution;
using FLink.Runtime.Query;
using FLink.Runtime.State.FileSystem;
using FLink.Runtime.State.TTL;

namespace FLink.Runtime.State.Memory
{
    /// <summary>
    /// This state backend holds the working state in the memory (JVM heap) of the TaskManagers. The state backend checkpoints state directly to the JobManager's memory (hence the backend's name), but the checkpoints will be persisted to a file system for high-availability setups and savepoints. The MemoryStateBackend is consequently a FileSystem-based backend that can work without a file system dependency in simple setups.
    /// </summary>
    public class MemoryStateBackend : AbstractFileStateBackend, IConfigurableStateBackend
    {
        /// <summary>
        /// The default maximal size that the snapshotted memory state may have (5 MiBytes).
        /// </summary>
        public static readonly int DefaultMaxStateSize = 5 * 1024 * 1024;

        /// <summary>
        /// Gets the maximum size that an individual state can have, as configured in the constructor (by default <see cref="DefaultMaxStateSize"/>).
        /// </summary>
        public int MaxStateSize;

        /// <summary>
        /// Gets whether the key/value data structures are asynchronously snapshotted.
        /// </summary>
        public TernaryBool AsynchronousSnapshots;

        #region [ Constructors ]  

        /// <summary>
        /// Creates a new memory state backend that accepts states whose serialized forms are up to the default state size (5 MB).
        /// Tips: Checkpoint and default savepoint locations are used as specified in the runtime configuration.
        /// </summary>
        public MemoryStateBackend()
            : this(string.Empty, string.Empty, DefaultMaxStateSize, TernaryBool.Undefined) { }

        /// <summary>
        /// Creates a new memory state backend that accepts states whose serialized forms are up to the default state size (5 MB). The state backend uses asynchronous snapshots or synchronous snapshots as configured.
        /// Tips: Checkpoint and default savepoint locations are used as specified in the runtime configuration.
        /// </summary>
        /// <param name="asynchronousSnapshots">Switch to enable asynchronous snapshots.</param>
        public MemoryStateBackend(bool asynchronousSnapshots)
        : this(string.Empty, string.Empty, DefaultMaxStateSize, asynchronousSnapshots.AsTernaryBool()) { }

        /// <summary>
        /// Creates a new memory state backend that accepts states whose serialized forms are up to the given number of bytes.
        /// Tips: Checkpoint and default savepoint locations are used as specified in the runtime configuration.
        /// WARNING: Increasing the size of this value beyond the default value <see cref="DefaultMaxStateSize"/> should be done with care.
        /// The checkpointed state needs to be send to the JobManager via limited size RPC messages, and there and the JobManager needs to be able to hold all aggregated state in its memory.
        /// </summary>
        /// <param name="maxStateSize">The maximal size of the serialized state</param>
        public MemoryStateBackend(int maxStateSize)
            : this(string.Empty, string.Empty, maxStateSize, TernaryBool.Undefined) { }

        /// <summary>
        /// Creates a new memory state backend that accepts states whose serialized forms are up to the given number of bytes and that uses asynchronous snashots as configured.
        /// Tips: Checkpoint and default savepoint locations are used as specified in the runtime configuration.
        /// WARNING: Increasing the size of this value beyond the default value <see cref="DefaultMaxStateSize"/> should be done with care.
        /// The checkpointed state needs to be send to the JobManager via limited size RPC messages, and there and the JobManager needs to be able to hold all aggregated state in its memory.
        /// </summary>
        /// <param name="maxStateSize">The maximal size of the serialized state</param>
        /// <param name="asynchronousSnapshots">Switch to enable asynchronous snapshots.</param>
        public MemoryStateBackend(int maxStateSize, bool asynchronousSnapshots)
            : this(string.Empty, string.Empty, maxStateSize, asynchronousSnapshots.AsTernaryBool()) { }

        /// <summary>
        /// Creates a new MemoryStateBackend, setting optionally the path to persist checkpoint metadata and savepoints to.
        /// </summary>
        /// <param name="checkpointPath">The path to write checkpoint metadata to. If null, the value from the runtime configuration will be used.</param>
        /// <param name="savepointPath">The path to write savepoints to. If null, the value from the runtime configuration will be used.</param>
        public MemoryStateBackend(string checkpointPath = default, string savepointPath = default)
            : this(checkpointPath, savepointPath, DefaultMaxStateSize, TernaryBool.Undefined) { }

        /// <summary>
        /// Creates a new MemoryStateBackend, setting optionally the paths to persist checkpoint metadata and savepoints to, as well as configuring state thresholds and asynchronous operations.
        /// WARNING: Increasing the size of this value beyond the default value <see cref="DefaultMaxStateSize"/> should be done with care.
        /// The checkpointed state needs to be send to the JobManager via limited size RPC messages, and there and the JobManager needs to be able to hold all aggregated state in its memory.
        /// </summary>
        /// <param name="checkpointPath">The path to write checkpoint metadata to. If null, the value from the runtime configuration will be used.</param>
        /// <param name="savepointPath">The path to write savepoints to. If null, the value from the runtime configuration will be used.</param>
        /// <param name="maxStateSize">The maximal size of the serialized state.</param>
        /// <param name="asynchronousSnapshots">Flag to switch between synchronous and asynchronous snapshot mode. If null, the value configured in the runtime configuration will be used.</param>
        public MemoryStateBackend(string checkpointPath, string savepointPath, int maxStateSize, TernaryBool asynchronousSnapshots)
        : base(string.IsNullOrWhiteSpace(checkpointPath) ? null : new FsPath(checkpointPath),
            string.IsNullOrWhiteSpace(savepointPath) ? null : new FsPath(savepointPath))
        {
            Preconditions.CheckArgument(maxStateSize > 0, "maxStateSize must be > 0");

            MaxStateSize = maxStateSize;
            AsynchronousSnapshots = asynchronousSnapshots;
        }

        #endregion

        #region [ Properties ]

        public bool IsUsingAsynchronousSnapshots => AsynchronousSnapshots.GetOrDefault(CheckpointingOptions.AsyncSnapshots.DefaultValue);

        #endregion

        public IStateBackend Configure(Configuration config)
        {
            throw new System.NotImplementedException();
        }

        #region [ Override Methods ]

        public override AbstractKeyedStateBackend<TKey> CreateKeyedStateBackend<TKey>(
            IEnvironment env, JobId jobId, 
            string operatorIdentifier,
            TypeSerializer<TKey> keySerializer, 
            int numberOfKeyGroups, 
            KeyGroupRange keyGroupRange,
            TaskKvStateRegistry kvStateRegistry, 
            ITtlTimeProvider ttlTimeProvider, 
            IMetricGroup metricGroup, 
            IList<IKeyedStateHandle> stateHandles,
            CloseableRegistry cancelStreamRegistry)
        {
            throw new System.NotImplementedException();
        }

        public override IOperatorStateBackend CreateOperatorStateBackend(
            IEnvironment env, 
            string operatorIdentifier, 
            IList<IOperatorStateHandle> stateHandles,
            CloseableRegistry cancelStreamRegistry) => new DefaultOperatorStateBackendBuilder(
            env.UserClassType,
            env.ExecutionConfig,
            IsUsingAsynchronousSnapshots,
            stateHandles,
            cancelStreamRegistry).Build();

        public override string ToString() => "MemoryStateBackend (data in heap memory/checkpoints to JobManager) " +
                                             "(checkpoints: '" + BaseCheckpointPath +
                                             "', savepoints: '" + BaseSavepointPath +
                                             "', asynchronous: " + AsynchronousSnapshots.GetName() +
                                             ", maxStateSize: " + MaxStateSize + ")";

        #endregion
    }
}
