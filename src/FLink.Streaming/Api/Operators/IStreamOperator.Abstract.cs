using FLink.Core.Api.Common;
using FLink.Core.Api.Common.State;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Exceptions;
using FLink.Extensions.DependencyInjection;
using FLink.Runtime.Checkpoint;
using FLink.Runtime.State;
using FLink.Streaming.Api.Watermarks;
using FLink.Streaming.Runtime.StreamRecords;
using FLink.Streaming.Runtime.Tasks;
using FLink.Streaming.Util;
using Microsoft.Extensions.Logging;
using System;
using FLink.Metrics.Core;
using FLink.Runtime.JobGraphs;
using FLink.Streaming.Api.Graph;
using FLink.Core.Api.CSharp.Functions;
using FLink.Runtime.Metrics.Groups;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// Base class for all stream operators. Operators that contain a user function should extend the class
    /// <see cref="AbstractUdfStreamOperator{TOut, TFunction}"/> instead(which is a specialized subclass of this class).
    /// For concrete implementations, one of the following two interfaces must also be implemented, to mark the operator as unary or binary:
    /// <see cref="IOneInputStreamOperator{TInput,TOutput}"/> or <see cref="ITwoInputStreamOperator{TInput1,TInput2,TOutput}"/>.
    /// Methods of <see cref="IStreamOperator{TOutput}"/> are guaranteed not to be called concurrently. Also, if using the timer service, timer callbacks are also guaranteed not to be called concurrently with methods on <see cref="IStreamOperator{TOutput}"/>.
    /// </summary>
    /// <typeparam name="TOutput">The output type of the operator</typeparam>
    public abstract class AbstractStreamOperator<TOutput> : IStreamOperator<TOutput>
    {
        /// <summary>
        /// The logger used by the operator class and its subclasses.
        /// </summary>
        public static ILogger Logger = ServiceLocator.GetService<ILogger<AbstractStreamOperator<TOutput>>>();

        /// <summary>
        /// A sane default for most operators
        /// </summary>
        public ChainingStrategy ChainingStrategy { get; set; } = ChainingStrategy.Head;

        /// <summary>
        /// The task that contains this operator (and other operators in the same chain).
        /// </summary>
        public StreamTask<TOutput, IStreamOperator<TOutput>> Container;

        public StreamConfig Config;

        public IOutput<StreamRecord<TOutput>> Output;

        public StreamingRuntimeContext RuntimeContext;

        #region [ key/value state ]

        /// <summary>
        /// <see cref="IKeySelector{TObject,TKey}"/> for extracting a key from an element being processed. This is used to scope keyed state to a key. This is null if the operator is not a keyed operator.
        /// This is for elements from the first input.
        /// </summary>
        private readonly IKeySelector<dynamic, dynamic> _stateKeySelector1;

        /// <summary>
        /// <see cref="IKeySelector{TObject,TKey}"/> for extracting a key from an element being processed. This is used to scope keyed state to a key. This is null if the operator is not a keyed operator.
        /// This is for elements from the second input.
        /// </summary>
        private readonly IKeySelector<dynamic, dynamic> _stateKeySelector2;

        /// <summary>
        /// Backend for keyed state. This might be empty if we're not on a keyed stream.
        /// </summary>
        public AbstractKeyedStateBackend<object> KeyedStateBackend;

        /// <summary>
        /// Keyed state store view on the keyed backend.
        /// </summary>
        public DefaultKeyedStateStore<object> KeyedStateStore;

        #endregion

        #region [ operator state ]

        /// <summary>
        /// Operator state backend / store.
        /// </summary>
        public IOperatorStateBackend OperatorStateBackend { get; }

        /// <summary>
        /// Metric group for the operator.
        /// </summary>
        public OperatorMetricGroup Metrics { get; }

        public LatencyStats LatencyStats { get; }

        #endregion

        #region [ time handler ]

        public IProcessingTimeService ProcessingTimeService;

        public InternalTimeServiceManager<object> TimeServiceManager;

        #endregion

        #region [ two-input operator watermarks ]

        private long _combinedWatermark = long.MaxValue;
        private long _input1Watermark = long.MaxValue;
        private long _input2Watermark = long.MaxValue;

        #endregion

        #region [ Life Cycle ]

        public virtual void Setup<TOut, TOperator>(StreamTask<TOut, TOperator> containingTask, StreamConfig config, IOutput<StreamRecord<TOutput>> output)
            where TOperator : IStreamOperator<TOut>
        {

        }

        public IMetricGroup MetricGroup { get; }

        public virtual void InitializeState()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is called immediately before any elements are processed, it should contain the operator's initialization logic, e.g. state initialization.
        /// </summary>
        /// <exception cref="System.Exception">An exception in this method causes the operator to fail.</exception>
        public abstract void Open();

        /// <summary>
        /// This method is called after all records have been added to the operators.
        /// The method is expected to flush all remaining buffered data. Exceptions during this flushing of buffered should be propagated, in order to cause the operation to be recognized asa failed, because the last data items are not processed properly.
        /// </summary>
        /// <exception cref="System.Exception">An exception in this method causes the operator to fail.</exception>
        public abstract void Close();

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }

        public void PrepareSnapshotPreBarrier(long checkpointId)
        {
            throw new NotImplementedException();
        }

        public virtual OperatorSnapshotFutures SnapshotState(long checkpointId, long timestamp, CheckpointOptions checkpointOptions,
            ICheckpointStreamFactory storageLocation)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region [ Properties and Services ]

        public ExecutionConfig ExecutionConfig => Container.ExecutionConfig;

        #endregion

        public virtual void NotifyCheckpointComplete(long checkpointId)
        {
            throw new NotImplementedException();
        }

        public void SetKeyContextElement1<T>(StreamRecord<T> record)
        {
            throw new NotImplementedException();
        }

        public void SetKeyContextElement2<T>(StreamRecord<T> record)
        {
            throw new NotImplementedException();
        }

        public OperatorId OperatorId { get; }

        protected TState GetPartitionedState<TState, TValue>(StateDescriptor<TState, TValue> stateDescriptor) where TState : IState =>
            GetPartitionedState(VoidNamespace.Instance, VoidNamespaceSerializer.Instance, stateDescriptor);

        protected TState GetPartitionedState<TState, TNamespace, TValue>(
            TNamespace @namespace,
            TypeSerializer<TNamespace> namespaceSerializer,
            StateDescriptor<TState, TValue> stateDescriptor) where TState : IState
        {
            /*
            TODO: NOTE: This method does a lot of work caching / retrieving states just to update the namespace.
            This method should be removed for the sake of namespaces being lazily fetched from the keyed
            state backend, or being set on the state directly.
            */

            if (KeyedStateStore == null)
                throw new RuntimeException("Cannot create partitioned state. The keyed state " +
                                           "backend has not been set. This indicates that the operator is not " +
                                           "partitioned/keyed.");

            return KeyedStateBackend.GetPartitionedState(@namespace, namespaceSerializer, stateDescriptor);
        }

        public virtual void ProcessWatermark(Watermark mark)
        {
            TimeServiceManager?.AdvanceWatermark(mark);
            Output.EmitWatermark(mark);
        }

        public virtual void ProcessLatencyMarker(LatencyMarker latencyMarker) => ReportOrForwardLatencyMarker(latencyMarker);

        protected void ReportOrForwardLatencyMarker(LatencyMarker marker)
        {
            // all operators are tracking latencies
            LatencyStats.ReportLatency(marker);
            // everything except sinks forwards latency markers
            Output.EmitLatencyMarker(marker);
        }

        public object CurrentKey { get; set; }
    }


}

