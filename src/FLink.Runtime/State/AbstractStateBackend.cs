using System;
using System.Collections.Generic;
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
    /// An abstract base implementation of the <see cref="IStateObject"/> interface.
    /// This class has currently no contents and only kept to not break the prior class hierarchy for users.
    /// </summary>
    public class AbstractStateBackend : IStateBackend
    {
        public ICompletedCheckpointStorageLocation ResolveCheckpoint(string externalPointer)
        {
            throw new NotImplementedException();
        }

        public ICheckpointStorage CreateCheckpointStorage(JobId jobId)
        {
            throw new NotImplementedException();
        }

        public AbstractKeyedStateBackend<TKey> CreateKeyedStateBackend<TKey>(IEnvironment env, JobId jobId, string operatorIdentifier,
            TypeSerializer<TKey> keySerializer, int numberOfKeyGroups, KeyGroupRange keyGroupRange,
            TaskKvStateRegistry kvStateRegistry, ITtlTimeProvider ttlTimeProvider, IMetricGroup metricGroup, IList<IKeyedStateHandle> stateHandles,
            CloseableRegistry cancelStreamRegistry)
        {
            throw new NotImplementedException();
        }

        public IOperatorStateBackend CreateOperatorStateBackend(IEnvironment env, string operatorIdentifier, IList<IOperatorStateBackend> stateHandles,
            CloseableRegistry cancelStreamRegistry)
        {
            throw new NotImplementedException();
        }
    }
}
