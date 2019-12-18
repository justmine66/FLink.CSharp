using System;
using System.Collections.Generic;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.Cache;
using FLink.Core.Api.Common.IO;
using FLink.Core.Api.Common.Operators;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Api.CSharp.Functions;
using FLink.Core.Api.CSharp.TypeUtils;
using FLink.Core.Api.Dag;
using FLink.Core.Exceptions;
using FLink.Core.Util;
using FLink.Extensions.DependencyInjection;
using FLink.Runtime.JobGraphs;
using FLink.Runtime.State;
using FLink.Streaming.Api.Collector.Selector;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Functions.Source;
using FLink.Streaming.Api.Operators;
using FLink.Streaming.Api.Transformations;
using FLink.Streaming.Runtime.Partitioners;
using FLink.Streaming.Runtime.Tasks;
using Microsoft.Extensions.Logging;

namespace FLink.Streaming.Api.Graph
{
    /// <summary>
    /// Class representing the streaming topology.
    /// It contains all the information necessary to build the job graph for the execution.
    /// </summary>
    public class StreamGraph : IPipeline
    {
        public static readonly ILogger Logger = ServiceLocator.GetService<ILogger<StreamGraph>>();

        public const string IterationSourceNamePrefix = "IterationSource";

        public const string IterationSinkNamePrefix = "IterationSink";

        public string JobName { get; set; }

        public ExecutionConfig ExecutionConfig { get; set; }
        public CheckpointConfig CheckpointConfig { get; set; }

        public SavepointRestoreSettings SavepointRestoreSettings { get; set; } = SavepointRestoreSettings.None;

        public ScheduleMode ScheduleMode { get; set; }

        public bool IsChainingEnabled { get; set; }

        public IList<(string, DistributedCache.DistributedCacheEntry)> UserArtifacts { get; set; }

        public TimeCharacteristic TimeCharacteristic { get; set; }

        /// <summary>
        /// If there are some stream edges that can not be chained and the shuffle mode of edge is not specified, translate these edges into BLOCKING result partition type.
        /// </summary>
        public bool IsBlockingConnectionsBetweenChains { get; set; }

        /// <summary>
        /// Flag to indicate whether to put all vertices into the same slot sharing group by default.
        /// </summary>
        public bool AllVerticesInSameSlotSharingGroupByDefault { get; set; } = true;

        public IDictionary<int, StreamNode> StreamNodes { get; set; }
        public ISet<int> Sources { get; set; }
        public ISet<int> Sinks { get; set; }
        public IDictionary<int, (int Id, List<string> Values)> VirtualSelectNodes { get; set; }
        public IDictionary<int, (int Id, OutputTag<object> OutputTag)> VirtualSideOutputNodes { get; set; }

        public IDictionary<int, (int Id, StreamPartitioner<object> Partitioner, ShuffleMode ShuffleMode)>
            VirtualPartitionNodes
        { get; set; }

        public IDictionary<int, string> VertexIDtoBrokerId { get; set; }
        public IDictionary<int, long> VertexIDtoLoopTimeout { get; set; }
        public IStateBackend StateBackend { get; set; }
        public ISet<(StreamNode, StreamNode)> IterationSourceSinkPairs { get; set; }

        public StreamGraph(ExecutionConfig executionConfig, CheckpointConfig checkpointConfig,
            SavepointRestoreSettings savepointRestoreSettings)
        {
            ExecutionConfig = Preconditions.CheckNotNull(executionConfig);
            CheckpointConfig = Preconditions.CheckNotNull(checkpointConfig);
            SavepointRestoreSettings = Preconditions.CheckNotNull(savepointRestoreSettings);

            // create an empty new stream graph.
            Clear();
        }

        public void Clear()
        {
            StreamNodes = new Dictionary<int, StreamNode>();
            VirtualSelectNodes = new Dictionary<int, (int, List<string>)>();
            VirtualSideOutputNodes = new Dictionary<int, (int, OutputTag<object>)>();
            VirtualPartitionNodes = new Dictionary<int, (int, StreamPartitioner<object>, ShuffleMode)>();
            VertexIDtoBrokerId = new Dictionary<int, string>();
            VertexIDtoLoopTimeout = new Dictionary<int, long>();
            IterationSourceSinkPairs = new HashSet<(StreamNode, StreamNode)>();
            Sources = new HashSet<int>();
            Sinks = new HashSet<int>();
        }

        public bool IsIterative => VertexIDtoLoopTimeout != null && VertexIDtoLoopTimeout.Values.Count >= 0;

        public void AddSource<TIn, TOut>(
            int vertexId,
            string slotSharingGroup,
            string coLocationGroup,
            IStreamOperatorFactory<TOut> operatorFactory,
            TypeInformation<TIn> inTypeInfo,
            TypeInformation<TOut> outTypeInfo,
            string operatorName)
        {
            AddOperator(vertexId, slotSharingGroup, coLocationGroup, operatorFactory, inTypeInfo, outTypeInfo,
                operatorName);
            Sources.Add(vertexId);
        }

        public void AddSink<TIn, TOut>(
            int vertexId,
            string slotSharingGroup,
            string coLocationGroup,
            IStreamOperatorFactory<TOut> operatorFactory,
            TypeInformation<TIn> inTypeInfo,
            TypeInformation<TOut> outTypeInfo,
            string operatorName)
        {
            AddOperator(vertexId, slotSharingGroup, coLocationGroup, operatorFactory, inTypeInfo, outTypeInfo,
                operatorName);
            Sinks.Add(vertexId);
        }

        public void AddOperator<TIn, TOut>(
            int vertexId,
            string slotSharingGroup,
            string coLocationGroup,
            IStreamOperatorFactory<TOut> operatorFactory,
            TypeInformation<TIn> inTypeInfo,
            TypeInformation<TOut> outTypeInfo,
            string operatorName)
        {
            var vertexType = operatorFactory.IsStreamSource
                ? typeof(SourceStreamTask<TOut, ISourceFunction<TOut>, StreamSource<TOut, ISourceFunction<TOut>>>)
                : typeof(OneInputStreamTask<object, TOut>);

            AddNode(vertexId, slotSharingGroup, coLocationGroup, vertexType,
                operatorFactory as IStreamOperatorFactory<object>, operatorName);

            var inSerializer = inTypeInfo == null || inTypeInfo is MissingTypeInfo
                ? null
                : inTypeInfo.CreateSerializer(ExecutionConfig);
            var outSerializer = outTypeInfo == null || outTypeInfo is MissingTypeInfo
                ? null
                : outTypeInfo.CreateSerializer(ExecutionConfig);

            SetSerializers(vertexId, inSerializer as TypeSerializer<object>, null,
                outSerializer as TypeSerializer<object>);

            if (operatorFactory.IsOutputTypeConfigurable && outTypeInfo != null)
            {
                // sets the output type which must be know at StreamGraph creation time
                operatorFactory.SetOutputType(outTypeInfo, ExecutionConfig);
            }

            if (operatorFactory.IsInputTypeConfigurable)
            {
                operatorFactory.SetInputType(inTypeInfo, ExecutionConfig);
            }

            if (Logger.IsEnabled(LogLevel.Debug))
            {
                Logger.LogDebug($"Vertex: {vertexId}");
            }
        }

        protected StreamNode AddNode(
            int vertexId,
            string slotSharingGroup,
            string coLocationGroup,
            Type vertexClass,
            IStreamOperatorFactory<object> operatorFactory,
            string operatorName)
        {
            if (StreamNodes.ContainsKey(vertexId))
            {
                throw new RuntimeException("Duplicate vertexID " + vertexId);
            }

            var vertex = new StreamNode(
                vertexId,
                slotSharingGroup,
                coLocationGroup,
                operatorFactory,
                operatorName,
                new List<IOutputSelector<object>>(),
                vertexClass);

            StreamNodes[vertexId] = vertex;

            return vertex;
        }

        public void SetSerializers(int vertexId, TypeSerializer<object> in1, TypeSerializer<object> in2,
            TypeSerializer<object> output)
        {
            var vertex = GetStreamNode(vertexId);
            vertex.TypeSerializerIn1 = in1;
            vertex.TypeSerializerIn2 = in2;
            vertex.TypeSerializerOut = output;
        }

        public StreamNode GetStreamNode(int vertexId) => StreamNodes[vertexId];

        public void SetBufferTimeout(int vertexId, long bufferTimeout)
        {
            if (GetStreamNode(vertexId) != null)
            {
                GetStreamNode(vertexId).BufferTimeout = bufferTimeout;
            }
        }

        public void SetTransformationUId(int nodeId, string transformationId)
        {
            var node = StreamNodes[nodeId];
            if (node != null)
            {
                node.TransformationUId = transformationId;
            }
        }

        public void SetTransformationUserHash(int nodeId, string nodeHash)
        {
            var node = StreamNodes[nodeId];
            if (node != null)
            {
                node.UserHash = nodeHash;
            }
        }

        public void SetResources(int vertexId, ResourceSpec minResources, ResourceSpec preferredResources)
        {
            if (GetStreamNode(vertexId) != null)
            {
                GetStreamNode(vertexId).SetResources(minResources, preferredResources);
            }
        }

        public void SetManagedMemoryWeight(int vertexId, int managedMemoryWeight)
        {
            if (GetStreamNode(vertexId) != null)
            {
                GetStreamNode(vertexId).ManagedMemoryWeight = managedMemoryWeight;
            }
        }

        /// <summary>
        /// Determines the slot sharing group of an operation across virtual nodes.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetSlotSharingGroup(int id)
        {
            if (VirtualSideOutputNodes.ContainsKey(id))
            {
                var mappedId = VirtualSideOutputNodes[id].Id;

                return GetSlotSharingGroup(mappedId);
            }

            if (VirtualSelectNodes.ContainsKey(id))
            {
                var mappedId = VirtualSelectNodes[id].Id;

                return GetSlotSharingGroup(mappedId);
            }

            if (VirtualPartitionNodes.ContainsKey(id))
            {
                var mappedId = VirtualPartitionNodes[id].Id;

                return GetSlotSharingGroup(mappedId);
            }

            var node = GetStreamNode(id);
            return node.SlotSharingGroup;
        }

        public void SetOutputFormat(int vertexId, IOutputFormat<object> outputFormat)
            => GetStreamNode(vertexId).OutputFormat = outputFormat;

        public void SetParallelism(int vertexId, int parallelism)
        {
            if (GetStreamNode(vertexId) != null)
            {
                GetStreamNode(vertexId).Parallelism = parallelism;
            }
        }

        public void SetMaxParallelism(int vertexId, int maxParallelism)
        {
            if (GetStreamNode(vertexId) != null)
            {
                GetStreamNode(vertexId).MaxParallelism = maxParallelism;
            }
        }

        public void SetOneInputStateKey(int vertexId, IKeySelector<object, object> keySelector,
            TypeSerializer<object> keySerializer)
        {
            var node = GetStreamNode(vertexId);

            node.StatePartitioner1 = keySelector;
            node.StateKeySerializer = keySerializer;
        }

        public void AddEdge(int upStreamVertexId, int downStreamVertexId, int typeNumber)
        {
            AddEdgeInternal(upStreamVertexId,
                downStreamVertexId,
                typeNumber,
                null,
                new List<string>(),
                null,
                ShuffleMode.Undefined);
        }

        public void AddVirtualPartitionNode(
            int originalId,
            int virtualId,
            StreamPartitioner<object> partitioner,
            ShuffleMode shuffleMode)
        {
            if (VirtualPartitionNodes.ContainsKey(virtualId))
            {
                throw new IllegalStateException("Already has virtual partition node with id " + virtualId);
            }

            VirtualPartitionNodes.Add(virtualId, (originalId, partitioner, shuffleMode));
        }

        private void AddEdgeInternal(
            int upStreamVertexId,
            int downStreamVertexId,
            int typeNumber,
            StreamPartitioner<object> partitioner,
            List<string> outputNames,
            OutputTag<object> outputTag,
            ShuffleMode shuffleMode)
        {
            if (VirtualSideOutputNodes.ContainsKey(upStreamVertexId))
            {
                var virtualId = upStreamVertexId;

                upStreamVertexId = VirtualSideOutputNodes[virtualId].Id;
                if (outputTag == null)
                {
                    outputTag = VirtualSideOutputNodes[virtualId].OutputTag;
                }

                AddEdgeInternal(upStreamVertexId, downStreamVertexId, typeNumber, partitioner, null, outputTag,
                    shuffleMode);
            }
            else if (VirtualSelectNodes.ContainsKey(upStreamVertexId))
            {
                var virtualId = upStreamVertexId;

                upStreamVertexId = VirtualSelectNodes[virtualId].Id;
                if (outputNames.Count <= 0)
                {
                    // selections that happen downstream override earlier selections
                    outputNames = VirtualSelectNodes[virtualId].Values;
                }

                AddEdgeInternal(upStreamVertexId, downStreamVertexId, typeNumber, partitioner, outputNames, outputTag,
                    shuffleMode);
            }
            else if (VirtualPartitionNodes.ContainsKey(upStreamVertexId))
            {
                var virtualId = upStreamVertexId;
                upStreamVertexId = VirtualPartitionNodes[virtualId].Id;
                if (partitioner == null)
                {
                    partitioner = VirtualPartitionNodes[virtualId].Partitioner;
                }

                shuffleMode = VirtualPartitionNodes[virtualId].ShuffleMode;

                AddEdgeInternal(upStreamVertexId, downStreamVertexId, typeNumber, partitioner, outputNames, outputTag,
                    shuffleMode);
            }
            else
            {
                var upstreamNode = GetStreamNode(upStreamVertexId);
                var downstreamNode = GetStreamNode(downStreamVertexId);

                switch (partitioner)
                {
                    // If no partitioner was specified and the parallelism of upstream and downstream
                    // operator matches use forward partitioning, use rebalance otherwise.
                    case null when upstreamNode.Parallelism == downstreamNode.Parallelism:
                        partitioner = new ForwardPartitioner<object>();
                        break;
                    case null:
                        partitioner = new RebalancePartitioner<object>();
                        break;
                }

                if (partitioner is ForwardPartitioner<object>)
                {
                    if (upstreamNode.Parallelism != downstreamNode.Parallelism)
                    {
                        throw new UnSupportedOperationException(
                            $"Forward partitioning does not allow change of parallelism. Upstream operation: {upstreamNode} parallelism: {upstreamNode.Parallelism}, downstream operation: {downstreamNode} parallelism: {downstreamNode.Parallelism} You must use another partitioning strategy, such as broadcast, rebalance, shuffle or global.");
                    }
                }

                var edge = new StreamEdge(
                    upstreamNode, 
                    downstreamNode, 
                    typeNumber, 
                    outputNames, 
                    partitioner, 
                    outputTag, 
                    shuffleMode);

                GetStreamNode(edge.SourceId).AddOutEdge(edge);
                GetStreamNode(edge.TargetId).AddOutEdge(edge);
            }
        }
    }
}
