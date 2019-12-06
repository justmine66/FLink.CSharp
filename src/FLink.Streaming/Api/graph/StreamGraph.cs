using System;
using System.Collections.Generic;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.Cache;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.Common.TypeUtils;
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
        public IDictionary<int, (int, List<string>)> VirtualSelectNodes { get; set; }
        public IDictionary<int, (int, OutputTag<object>)> VirtualSideOutputNodes { get; set; }
        public IDictionary<int, (int, StreamPartitioner<object>, ShuffleMode)> VirtualPartitionNodes { get; set; }

        public IDictionary<int, string> VertexIDtoBrokerID { get; set; }
        public IDictionary<int, long> VertexIDtoLoopTimeout { get; set; }
        public IStateBackend StateBackend { get; set; }
        public ISet<(StreamNode, StreamNode)> IterationSourceSinkPairs { get; set; }

        public StreamGraph(ExecutionConfig executionConfig, CheckpointConfig checkpointConfig, SavepointRestoreSettings savepointRestoreSettings)
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
            VertexIDtoBrokerID = new Dictionary<int, string>();
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
            AddOperator(vertexId, slotSharingGroup, coLocationGroup, operatorFactory, inTypeInfo, outTypeInfo, operatorName);
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
            AddOperator(vertexId, slotSharingGroup, coLocationGroup, operatorFactory, inTypeInfo, outTypeInfo, operatorName);
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

            AddNode(vertexId, slotSharingGroup, coLocationGroup, vertexType, operatorFactory as IStreamOperatorFactory<object>, operatorName);

            var inSerializer = inTypeInfo == null || inTypeInfo is MissingTypeInfo ? null : inTypeInfo.CreateSerializer(ExecutionConfig);
            var outSerializer = outTypeInfo == null || outTypeInfo is MissingTypeInfo ? null : outTypeInfo.CreateSerializer(ExecutionConfig);

            SetSerializers(vertexId, inSerializer as TypeSerializer<object>, null, outSerializer as TypeSerializer<object>);

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

        public void SetSerializers(int vertexId, TypeSerializer<object> in1, TypeSerializer<object> in2, TypeSerializer<object> output)
        {
            var vertex = GetStreamNode(vertexId);
            vertex.TypeSerializerIn1 = in1;
            vertex.TypeSerializerIn2 = in2;
            vertex.TypeSerializerOut = output;
        }

        public StreamNode GetStreamNode(int vertexId) => StreamNodes[vertexId];
    }
}
