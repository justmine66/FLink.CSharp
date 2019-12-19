using FLink.Core.Api.Common;
using FLink.Core.Api.Common.Cache;
using FLink.Core.Api.CSharp.Functions;
using FLink.Core.Api.Dag;
using FLink.Core.Exceptions;
using FLink.Core.Util;
using FLink.Extensions.DependencyInjection;
using FLink.Runtime.JobGraphs;
using FLink.Runtime.State;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Operators;
using FLink.Streaming.Api.Transformations;
using FLink.Streaming.Runtime.Partitioners;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace FLink.Streaming.Api.Graph
{
    /// <summary>
    /// A generator that generates a <see cref="StreamGraph"/> from a graph of <see cref="tr"/>s.
    /// </summary>
    public class StreamGraphGenerator
    {
        private static readonly ILogger Logger = ServiceLocator.GetService<ILogger<StreamGraphGenerator>>();

        public static readonly int DefaultLowerBoundMaxParallelism = KeyGroupRangeAssignment.DefaultLowerBoundMaxParallelism;
        public static readonly ScheduleMode DefaultScheduleMode = ScheduleMode.Eager;
        public static readonly TimeCharacteristic DefaultTimeCharacteristic = TimeCharacteristic.ProcessingTime;
        public static readonly string DefaultJobName = "Flink Streaming Job";
        /// <summary>
        /// The default buffer timeout (max delay of records in the network stack).
        /// </summary>
        public static readonly long DefaultNetworkBufferTimeout = 100L;
        public static readonly string DefaultSlotSharingGroup = "default";

        private readonly IList<Transformation<object>> _transformations;
        private readonly ExecutionConfig _executionConfig;
        private readonly CheckpointConfig _checkpointConfig;
        private SavepointRestoreSettings _savepointRestoreSettings = SavepointRestoreSettings.None;
        private IStateBackend _stateBackend;
        private bool _chaining = true;
        private ScheduleMode _scheduleMode = DefaultScheduleMode;
        private IList<(string, DistributedCacheEntry)> _userArtifacts;
        private TimeCharacteristic _timeCharacteristic = DefaultTimeCharacteristic;
        private long _defaultBufferTimeout = DefaultNetworkBufferTimeout;
        private string _jobName = DefaultJobName;
        /// <summary>
        /// If there are some stream edges that can not be chained and the shuffle mode of edge is not specified, translate these edges into BLOCKING result partition type.
        /// </summary>
        private bool _blockingConnectionsBetweenChains = false;

        // This is used to assign a unique ID to iteration source/sink
        protected static int _iterationIdCounter = 0;
        public static int NewIterationNodeId
        {
            get
            {
                _iterationIdCounter--;
                return _iterationIdCounter;
            }
        }

        private StreamGraph _streamGraph;
        // Keep track of which Transforms we have already transformed, this is necessary because we have loops, i.e. feedback edges.
        private IDictionary<Transformation<object>, IList<int>> _alreadyTransformed;

        public StreamGraphGenerator(IList<Transformation<object>> transformations, ExecutionConfig executionConfig, CheckpointConfig checkpointConfig)
        {
            _transformations = Preconditions.CheckNotNull(transformations);
            _executionConfig = Preconditions.CheckNotNull(executionConfig);
            _checkpointConfig = Preconditions.CheckNotNull(checkpointConfig);
        }

        public StreamGraphGenerator SetStateBackend(IStateBackend stateBackend)
        {
            _stateBackend = stateBackend;
            return this;
        }

        public StreamGraphGenerator SetChaining(bool chaining)
        {
            _chaining = chaining;
            return this;
        }

        public StreamGraphGenerator SetScheduleMode(ScheduleMode scheduleMode)
        {
            _scheduleMode = scheduleMode;
            return this;
        }

        public StreamGraphGenerator SetUserArtifacts(IList<(string, DistributedCacheEntry)> userArtifacts)
        {
            _userArtifacts = userArtifacts;
            return this;
        }

        public StreamGraphGenerator SetTimeCharacteristic(TimeCharacteristic timeCharacteristic)
        {
            _timeCharacteristic = timeCharacteristic;
            return this;
        }

        public StreamGraphGenerator SetDefaultBufferTimeout(long defaultBufferTimeout)
        {
            _defaultBufferTimeout = defaultBufferTimeout;
            return this;
        }

        public StreamGraphGenerator SetJobName(string jobName)
        {
            _jobName = jobName;
            return this;
        }

        public StreamGraphGenerator SetBlockingConnectionsBetweenChains(bool blockingConnectionsBetweenChains)
        {
            _blockingConnectionsBetweenChains = blockingConnectionsBetweenChains;
            return this;
        }

        public void SetSavepointRestoreSettings(SavepointRestoreSettings savepointRestoreSettings)
        {
            _savepointRestoreSettings = savepointRestoreSettings;
        }

        public StreamGraph Generate()
        {
            _streamGraph = new StreamGraph(_executionConfig, _checkpointConfig, _savepointRestoreSettings)
            {
                StateBackend = _stateBackend,
                IsChainingEnabled = _chaining,
                ScheduleMode = _scheduleMode,
                UserArtifacts = _userArtifacts,
                TimeCharacteristic = _timeCharacteristic,
                JobName = _jobName,
                IsBlockingConnectionsBetweenChains = _blockingConnectionsBetweenChains
            };

            _alreadyTransformed = new Dictionary<Transformation<object>, IList<int>>();

            foreach (var transformation in _transformations)
            {
                Transform(transformation);
            }

            var builtStreamGraph = _streamGraph;

            _alreadyTransformed.Clear();
            _alreadyTransformed = null;
            _streamGraph = null;

            return builtStreamGraph;
        }

        /// <summary>
        /// Transforms one <see cref="Transformation{TElement}"/>.
        /// This checks whether we already transformed it and exits early in that case.
        /// If not it delegates to one of the transformation specific methods.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        private IList<int> Transform(Transformation<object> transform)
        {
            if (_alreadyTransformed.ContainsKey(transform))
            {
                return _alreadyTransformed[transform];
            }

            Logger.LogDebug($"Transforming {transform}");

            if (transform.MaxParallelism <= 0)
            {
                // if the max parallelism hasn't been set, then first use the job wide max parallelism from the ExecutionConfig.
                var globalMaxParallelismFromConfig = _executionConfig.MaxParallelism;
                if (globalMaxParallelismFromConfig > 0)
                {
                    transform.MaxParallelism = globalMaxParallelismFromConfig;
                }
            }

            IList<int> transformedIds;

            switch (transform)
            {
                case OneInputTransformation<object, object> transformation:
                    transformedIds = TransformOneInputTransform(transformation);
                    break;
                case TwoInputTransformation<object, object, object> transformation:
                    transformedIds = TransformTwoInputTransform(transformation);
                    break;
                case SourceTransformation<object> transformation:
                    transformedIds = TransformSource(transformation);
                    break;
                case SinkTransformation<object> transformation:
                    transformedIds = TransformSink(transformation);
                    break;
                case SplitTransformation<object> transformation:
                    transformedIds = TransformSplit(transformation);
                    break;
                case UnionTransformation<object> transformation:
                    transformedIds = TransformUnion(transformation);
                    break;
                case SelectTransformation<object> transformation:
                    transformedIds = TransformSelect(transformation);
                    break;
                case FeedbackTransformation<object> transformation:
                    transformedIds = TransformFeedback(transformation);
                    break;
                case CoFeedbackTransformation<object> transformation:
                    transformedIds = TransformCoFeedback(transformation);
                    break;
                case PartitionTransformation<object> transformation:
                    transformedIds = TransformPartition(transformation);
                    break;
                case SideOutputTransformation<object> transformation:
                    transformedIds = TransformSideOutput(transformation);
                    break;
                default:
                    throw new IllegalStateException("Unknown transformation: " + transform);
            }

            if (!_alreadyTransformed.ContainsKey(transform))
            {
                _alreadyTransformed.Add(transform, transformedIds);
            }

            var bufferTimeout = transform.BufferTimeout >= 0 ? transform.BufferTimeout : _defaultBufferTimeout;
            _streamGraph.SetBufferTimeout(transform.Id, bufferTimeout);

            if (transform.UId != null)
            {
                _streamGraph.SetTransformationUId(transform.Id, transform.UId);
            }

            if (transform.UserProvidedNodeHash != null)
            {
                _streamGraph.SetTransformationUserHash(transform.Id, transform.UserProvidedNodeHash);
            }

            if (!_streamGraph.ExecutionConfig.EnableAutoGeneratedUIds)
            {
                if (transform is PhysicalTransformation<object> &&
                    transform.UserProvidedNodeHash == null &&
                    transform.UId == null)
                {
                    throw new IllegalStateException(
                        $"Auto generated UIDs have been disabled but no UID or hash has been assigned to operator {transform.Name}");
                }
            }

            if (transform.MinResources != null && transform.PreferredResources != null)
            {
                _streamGraph.SetResources(transform.Id, transform.MinResources, transform.PreferredResources);
            }

            _streamGraph.SetManagedMemoryWeight(transform.Id, transform.ManagedMemoryWeight);

            return transformedIds;
        }

        private IList<int> TransformSideOutput(SideOutputTransformation<object> sideOutput)
        {
            var input = sideOutput.Input;
            var resultIds = Transform(input);

            // the recursive transform might have already transformed this
            if (_alreadyTransformed.ContainsKey(sideOutput))
            {
                return _alreadyTransformed[sideOutput];
            }

            var virtualResultIds = new List<int>();

            foreach (var inputId in resultIds)
            {
                var virtualId = Transformation<object>.NewNodeId;
                _streamGraph.AddVirtualSideOutputNode(inputId, virtualId, sideOutput.OutputTag);
                virtualResultIds.Add(virtualId);
            }

            return virtualResultIds;
        }

        private IList<int> TransformPartition<T>(PartitionTransformation<T> partition)
        {
            var input = partition.Input as Transformation<object>;
            var resultIds = new List<int>();

            var transformedIds = Transform(input);
            var partitioner = partition.Partitioner as StreamPartitioner<object>;

            foreach (var transformedId in transformedIds)
            {
                var virtualId = Transformation<object>.NewNodeId;
                _streamGraph.AddVirtualPartitionNode(transformedId, virtualId, partitioner, partition.ShuffleMode);
                resultIds.Add(virtualId);
            }

            return resultIds;
        }

        private IList<int> TransformCoFeedback(CoFeedbackTransformation<object> coFeedback)
        {
            throw new NotImplementedException();
        }

        private IList<int> TransformFeedback(FeedbackTransformation<object> feedback)
        {
            throw new NotImplementedException();
        }

        private IList<int> TransformSelect<T>(SelectTransformation<T> select)
        {
            var input = select.Input as Transformation<object>;
            var resultIds = Transform(input);

            // the recursive transform might have already transformed this
            if (_alreadyTransformed.ContainsKey(input))
            {
                return _alreadyTransformed[input];
            }

            var virtualResultIds = new List<int>();

            foreach (var inputId in resultIds)
            {
                var virtualId = Transformation<object>.NewNodeId;
                _streamGraph.AddVirtualSelectNode(inputId, virtualId, select.SelectedNames);
                virtualResultIds.Add(virtualId);
            }

            return virtualResultIds;
        }

        /// <summary>
        /// Transforms a <see cref="TransformSplit"/>.
        /// We add the output selector to previously transformed nodes.
        /// </summary>
        private IList<int> TransformSplit(SplitTransformation<object> split)
        {
            var input = split.Input as Transformation<object>;
            var resultIds = Transform(input);

            ValidateSplitTransformation(input);

            // the recursive transform call might have transformed this already
            if (_alreadyTransformed.ContainsKey(split))
            {
                return _alreadyTransformed[split];
            }

            foreach (var inputId in resultIds)
            {
                _streamGraph.AddOutputSelector(inputId, split.OutputSelector);
            }

            return resultIds;
        }

        /// <summary>
        /// Transforms a <see cref="UnionTransformation{T}"/>.
        /// This is easy, we only have to transform the inputs and return all the IDs in a list so that downstream operations can connect to all upstream nodes.
        /// </summary>
        /// <param name="union"></param>
        /// <returns></returns>
        private IList<int> TransformUnion(UnionTransformation<object> union)
        {
            var inputs = union.Inputs;
            var resultIds = new List<int>();

            foreach (var input in inputs)
            {
                resultIds.AddRange(Transform(input));
            }

            return resultIds;
        }

        private IList<int> TransformSink(SinkTransformation<object> sink)
        {
            var inputIds = Transform(sink.Input);
            var slotSharingGroup = DetermineSlotSharingGroup(sink.SlotSharingGroup, inputIds);

            _streamGraph.AddSink(sink.Id,
                slotSharingGroup,
                sink.CoLocationGroupKey,
                sink.OperatorFactory,
                sink.Input.OutputType,
                null,
                $"Sink: {sink.Name}");

            var operatorFactory = sink.OperatorFactory;
            if (operatorFactory is IOutputFormatOperatorFactory<object> factory)
            {
                _streamGraph.SetOutputFormat(sink.Id, factory.OutputFormat);
            }

            var parallelism = sink.Parallelism != ExecutionConfig.DefaultParallelism
                ? sink.Parallelism
                : _executionConfig.Parallelism;

            _streamGraph.SetParallelism(sink.Id, parallelism);
            _streamGraph.SetMaxParallelism(sink.Id, sink.MaxParallelism);

            foreach (var id in inputIds)
            {
                _streamGraph.AddEdge(id, sink.Id, 0);
            }

            if (sink.StateKeySelector != null)
            {
                var keySerializer = sink.StateKeyType.CreateSerializer(_executionConfig);
                _streamGraph.SetOneInputStateKey(sink.Id, sink.StateKeySelector, keySerializer);
            }

            return new List<int>();
        }

        private IList<int> TransformSource(SourceTransformation<object> source)
        {
            var slotSharingGroup = DetermineSlotSharingGroup(source.SlotSharingGroup, new List<int>());

            _streamGraph.AddSource<object, object>(source.Id,
                slotSharingGroup,
                source.CoLocationGroupKey,
                source.OperatorFactory,
                null,
                source.OutputType,
                $"Source: {source.Name}");

            if (source.OperatorFactory is IInputFormatOperatorFactory<object> factory)
            {
                _streamGraph.SetInputFormat(source.Id, factory.InputFormat);
            }

            var parallelism = source.Parallelism != ExecutionConfig.DefaultParallelism ?
                source.Parallelism : _executionConfig.Parallelism;
            _streamGraph.SetParallelism(source.Id, parallelism);
            _streamGraph.SetMaxParallelism(source.Id, source.MaxParallelism);

            var result = SingletonList<int>.Instance;
            result.Add(source.Id);

            return result;
        }

        private IList<int> TransformTwoInputTransform(TwoInputTransformation<object, object, object> transform)
        {
            var inputIds1 = Transform(transform.Input1);
            var inputIds2 = Transform(transform.Input2);

            // the recursive call might have already transformed this
            if (_alreadyTransformed.ContainsKey(transform))
            {
                return _alreadyTransformed[transform];
            }

            var allInputIds = new List<int>();
            allInputIds.AddRange(inputIds1);
            allInputIds.AddRange(inputIds2);

            var slotSharingGroup = DetermineSlotSharingGroup(transform.SlotSharingGroup, allInputIds);

            _streamGraph.AddCoOperator(
                transform.Id,
                slotSharingGroup,
                transform.CoLocationGroupKey,
                transform.OperatorFactory,
                transform.InputType1,
                transform.InputType2,
                transform.OutputType,
                transform.Name);

            if (transform.StateKeySelector1 != null || transform.StateKeySelector2 != null)
            {
                var keySerializer = transform.StateKeyType.CreateSerializer(_executionConfig);

                _streamGraph.SetTwoInputStateKey(transform.Id, transform.StateKeySelector1, transform.StateKeySelector2, keySerializer);
            }

            var parallelism = transform.Parallelism != ExecutionConfig.DefaultParallelism
                ? transform.Parallelism
                : _executionConfig.Parallelism;
            _streamGraph.SetParallelism(transform.Id, parallelism);
            _streamGraph.SetMaxParallelism(transform.Id, transform.MaxParallelism);

            foreach (var inputId in inputIds1)
            {
                _streamGraph.AddEdge(inputId, transform.Id, 1);
            }

            foreach (var inputId in inputIds2)
            {
                _streamGraph.AddEdge(inputId, transform.Id, 2);
            }

            var result = SingletonList<int>.Instance;
            result.Add(transform.Id);
            return result;
        }

        private IList<int> TransformOneInputTransform<TInput, TOutput>(OneInputTransformation<TInput, TOutput> transform)
        {
            var input = transform.Input as Transformation<object>;
            var inputIds = Transform(input);

            // the recursive call might have already transformed this
            if (_alreadyTransformed.ContainsKey(input))
            {
                return _alreadyTransformed[input];
            }

            var slotSharingGroup = DetermineSlotSharingGroup(input.SlotSharingGroup, inputIds);

            _streamGraph.AddOperator(transform.Id,
                slotSharingGroup,
                transform.CoLocationGroupKey,
                transform.OperatorFactory,
                transform.InputType,
                transform.OutputType,
                transform.Name);

            if (transform.StateKeySelector != null)
            {
                var keySerializer = transform.StateKeyType.CreateSerializer(_executionConfig);
                var selector = transform.StateKeySelector as IKeySelector<object, object>;

                _streamGraph.SetOneInputStateKey(transform.Id, selector, keySerializer);
            }

            var parallelism = transform.Parallelism != ExecutionConfig.DefaultParallelism ?
                transform.Parallelism : _executionConfig.Parallelism;
            _streamGraph.SetParallelism(transform.Id, parallelism);
            _streamGraph.SetMaxParallelism(transform.Id, transform.MaxParallelism);

            foreach (var inputId in inputIds)
            {
                _streamGraph.AddEdge(inputId, transform.Id, 0);
            }

            var result = SingletonList<int>.Instance;
            result.Add(transform.Id);
            return result;
        }

        /// <summary>
        /// Determines the slot sharing group for an operation based on the slot sharing group set by the user and the slot sharing groups of the inputs. 
        /// If the user specifies a group name, this is taken as is. If nothing is specified and the input operations all have the same group name then this name is taken. Otherwise the default group is chosen.
        /// </summary>
        /// <param name="specifiedGroup">The group specified by the user.</param>
        /// <param name="inputIds">The IDs of the input operations.</param>
        /// <returns></returns>
        private string DetermineSlotSharingGroup(string specifiedGroup, IList<int> inputIds)
        {
            if (specifiedGroup != null)
            {
                return specifiedGroup;
            }

            string inputGroup = null;
            foreach (var id in inputIds)
            {
                var inputGroupCandidate = _streamGraph.GetSlotSharingGroup(id);
                if (inputGroup == null)
                {
                    inputGroup = inputGroupCandidate;
                }
                else if (!inputGroup.Equals(inputGroupCandidate))
                {
                    return DefaultSlotSharingGroup;
                }
            }

            return inputGroup ?? DefaultSlotSharingGroup;
        }

        private void ValidateSplitTransformation<T>(Transformation<T> input)
        {
            if (input is SelectTransformation<T> || input is SplitTransformation<T>)
            {
                throw new IllegalStateException("Consecutive multiple splits are not supported. Splits are deprecated. Please use side-outputs.");
            }

            if (input is SideOutputTransformation<T>)
            {
                throw new IllegalStateException("Split after side-outputs are not supported. Splits are deprecated. Please use side-outputs.");
            }

            switch (input)
            {
                case UnionTransformation<T> union:
                    {
                        foreach (var transformation in union.Inputs)
                        {
                            ValidateSplitTransformation(transformation);
                        }

                        break;
                    }

                case PartitionTransformation<T> partition:
                    ValidateSplitTransformation(partition.Input);
                    break;
            }
        }
    }
}
