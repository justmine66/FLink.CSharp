using System.Collections.Generic;
using System.Threading.Tasks;
using FLink.Core.Api.Common.State;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.IO;
using FLink.Runtime.Checkpoint;

namespace FLink.Runtime.State
{
    public abstract class AbstractKeyedStateBackend<TKey> : IKeyedStateBackend<TKey>, ISnapshotStrategy<SnapshotResult<IKeyedStateHandle>>, ICloseable, ICheckpointListener
    {
        public TKey CurrentKey { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void ApplyToAllKeys<TNamespace, TState, T>(TNamespace @namespace, TypeSerializer<TNamespace> namespaceSerializer, StateDescriptor<TState, T> stateDescriptor, IKeyedStateFunction<TKey, TState> function) where TState : IState
        {
            throw new System.NotImplementedException();
        }

        public abstract IEnumerable<TKey> GetKeys<TNamespace>(string state, TNamespace @namespace);

        public void Close()
        {
            throw new System.NotImplementedException();
        }

        public bool DeRegisterKeySelectionListener(IKeySelectionListener<TKey> listener)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public TypeSerializer<TKey> GetKeySerializer()
        {
            throw new System.NotImplementedException();
        }

        public TState GetOrCreateKeyedState<TNamespace, TState, T>(TypeSerializer<TNamespace> namespaceSerializer, StateDescriptor<TState, T> stateDescriptor) where TState : IState
        {
            throw new System.NotImplementedException();
        }

        public TState GetPartitionedState<TNamespace, TState, TValue>(TNamespace @namespace, TypeSerializer<TNamespace> namespaceSerializer, StateDescriptor<TState, TValue> stateDescriptor) where TState : IState
        {
            throw new System.NotImplementedException();
        }

        public void NotifyCheckpointComplete(long checkpointId)
        {
            throw new System.NotImplementedException();
        }

        public void RegisterKeySelectionListener(IKeySelectionListener<TKey> listener)
        {
            throw new System.NotImplementedException();
        }

        public TaskCompletionSource<SnapshotResult<IKeyedStateHandle>> Snapshot(long checkpointId, long timestamp, ICheckpointStreamFactory streamFactory, CheckpointOptions checkpointOptions)
        {
            throw new System.NotImplementedException();
        }
    }
}
