using System.Collections.Generic;
using FLink.Core.Api.Common.Accumulators;
using FLink.Core.Api.Common.Functions.Util;
using FLink.Core.Api.Common.State;
using FLink.Core.Exceptions;
using FLink.Core.Util;
using FLink.Runtime.Execution;
using FLink.Runtime.JobGraphs.Tasks;
using FLink.Runtime.TaskExecutors;
using FLink.Streaming.Api.Graphs;
using FLink.Streaming.Runtime.Tasks;

namespace FLink.Streaming.Api.Operators
{
    public class StreamingRuntimeContext : AbstractRuntimeUdfContext
    {
        /// <summary>
        /// The operator to which this function belongs.
        /// </summary>
        private readonly AbstractStreamOperator<object> _operator;

        /// <summary>
        /// The task environment running the operator.
        /// </summary>
        private readonly IEnvironment _taskEnvironment;

        private readonly StreamConfig _streamConfig;

        public StreamingRuntimeContext(
            AbstractStreamOperator<object> @operator,
            IEnvironment env,
            IDictionary<string, IAccumulator<object, object>> accumulators)
            : base(env.TaskInfo, @operator.ExecutionConfig, accumulators, env.DistributedCacheEntries, @operator.MetricGroup)
        {
            _operator = @operator;
            _taskEnvironment = env;
            _streamConfig = new StreamConfig(env.TaskConfiguration);
            OperatorUniqueId = _operator.OperatorId.ToString();
        }

        /// <summary>
        /// Gets the input split provider associated with the operator.
        /// </summary>
        public IInputSplitProvider InputSplitProvider => _taskEnvironment.InputSplitProvider;

        public IProcessingTimeService ProcessingTimeService => _operator.ProcessingTimeService;

        /// <summary>
        /// Gets the global aggregate manager for the current job.
        /// </summary>
        public IGlobalAggregateManager GlobalAggregateManager => _taskEnvironment.GlobalAggregateManager;

        /// <summary>
        /// Gets representation of the operator's unique id.
        /// Returned value is guaranteed to be unique between operators within the same job and to be stable and the same across job submissions.
        /// This operation is currently only supported in Streaming (DataStream) contexts.
        /// </summary>
        public string OperatorUniqueId { get; }

        public override bool HasBroadcastVariable(string name)
        {
            throw new UnSupportedOperationException("Broadcast variables can only be used in DataSet programs");
        }

        public override IValueState<TValue> GetState<TValue>(ValueStateDescriptor<TValue> stateProperties)
        {
            var keyedStateStore = CheckPreconditionsAndGetKeyedStateStore(stateProperties);
            stateProperties.InitializeSerializerUnlessSet(ExecutionConfig);

            return keyedStateStore.GetState(stateProperties);
        }

        public override IListState<TValue> GetListState<TValue>(ListStateDescriptor<TValue> stateProperties)
        {
            var keyedStateStore = CheckPreconditionsAndGetKeyedStateStore(stateProperties);
            stateProperties.InitializeSerializerUnlessSet(ExecutionConfig);

            return keyedStateStore.GetListState(stateProperties);
        }

        public override IAggregatingState<TInput, TOutput> GetAggregatingState<TInput, TAccumulator, TOutput>(AggregatingStateDescriptor<TInput, TAccumulator, TOutput> stateProperties)
        {
            var keyedStateStore = CheckPreconditionsAndGetKeyedStateStore(stateProperties);
            stateProperties.InitializeSerializerUnlessSet(ExecutionConfig);

            return keyedStateStore.GetAggregatingState(stateProperties);
        }

        public override IReducingState<TValue> GetReducingState<TValue>(ReducingStateDescriptor<TValue> stateProperties)
        {
            var keyedStateStore = CheckPreconditionsAndGetKeyedStateStore(stateProperties);
            stateProperties.InitializeSerializerUnlessSet(ExecutionConfig);

            return keyedStateStore.GetReducingState(stateProperties);
        }

        public override IMapState<TKey, TValue> GetMapState<TKey, TValue>(MapStateDescriptor<TKey, TValue> stateProperties)
        {
            var keyedStateStore = CheckPreconditionsAndGetKeyedStateStore(stateProperties);
            stateProperties.InitializeSerializerUnlessSet(ExecutionConfig);

            return keyedStateStore.GetMapState(stateProperties);
        }

        private IKeyedStateStore CheckPreconditionsAndGetKeyedStateStore<TState, TValue>(StateDescriptor<TState, TValue> stateDescriptor) where TState : IState
        {
            Preconditions.CheckNotNull(stateDescriptor, "The state properties must not be null");

            var keyedStateStore = _operator.KeyedStateStore;
            Preconditions.CheckNotNull(keyedStateStore, "Keyed state can only be used on a 'keyed stream', i.e., after a 'keyBy()' operation.");

            return keyedStateStore;
        }

        /// <summary>
        /// Gets true if checkpointing is enabled for the running job.
        /// </summary>
        public bool IsCheckpointingEnabled => _streamConfig.IsCheckpointingEnabled;

        /// <summary>
        /// Gets the checkpointing mode.
        /// </summary>
        public CheckpointingMode CheckpointMode => _streamConfig.CheckpointMode;

        public long BufferTimeout => _streamConfig.BufferTimeout;
    }
}
