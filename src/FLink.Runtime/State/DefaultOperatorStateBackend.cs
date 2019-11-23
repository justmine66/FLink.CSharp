using FLink.Core.Api.Common;
using FLink.Core.Api.Common.State;
using FLink.Core.FS;
using FLink.Runtime.Checkpoint;
using System.Collections.Generic;
using System.Threading.Tasks;
using FLink.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FLink.Runtime.State
{
    /// <summary>
    /// Default implementation of OperatorStateStore that provides the ability to make snapshots.
    /// </summary>
    public class DefaultOperatorStateBackend : IOperatorStateBackend
    {
        private static readonly ILogger Logger = ServiceLocator.GetService<ILogger<DefaultOperatorStateBackend>>();

        /// <summary>
        /// The default namespace for state in cases where no state name is provided
        /// </summary>
        public static readonly string DefaultOperatorStateName = "_default_";

        // Map for all registered operator states. Maps state name -> state
        private readonly Dictionary<string, PartitionableListState<object>> _registeredOperatorStates;
        // Map for all registered operator broadcast states. Maps state name -> state
        private readonly Dictionary<string, IBackendWritableBroadcastState<object, object>> _registeredBroadcastStates;
        // CloseableRegistry to participate in the tasks lifecycle.
        private readonly CloseableRegistry _closeStreamOnCancelRegistry;
        // The execution configuration.
        private readonly ExecutionConfig _executionConfig;

        /// <summary>
        /// Cache of already accessed states.
        /// <p>In contrast to <see cref="_registeredOperatorStates"/> which may be repopulated with restored state, this map is always empty at the beginning.
        /// </summary>
        private readonly Dictionary<string, PartitionableListState<object>> _accessedStatesByName;

        public IBroadcastState<TKey, TValue> GetBroadcastState<TKey, TValue>(MapStateDescriptor<TKey, TValue> stateDescriptor)
        {
            throw new System.NotImplementedException();
        }

        public IListState<TValue> GetListState<TValue>(ListStateDescriptor<TValue> stateDescriptor)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> RegisteredStateNames { get; }
        public IEnumerable<string> RegisteredBroadcastStateNames { get; }

        public TaskCompletionSource<SnapshotResult<IOperatorStateHandle>> Snapshot(long checkpointId, long timestamp, ICheckpointStreamFactory streamFactory,
            CheckpointOptions checkpointOptions)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
