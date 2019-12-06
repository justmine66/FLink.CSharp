using System;
using System.Collections.Generic;
using System.Linq;
using FLink.Core.Api.Common.IO;
using FLink.Core.Api.Common.Operators;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Api.CSharp.Functions;
using FLink.Core.Api.Dag;
using FLink.Core.Exceptions;
using FLink.Core.IO;
using FLink.Streaming.Api.Collector.Selector;
using FLink.Streaming.Api.Operators;

namespace FLink.Streaming.Api.Graph
{
    /// <summary>
    /// Representing the operators in the streaming programs, with all their properties.
    /// </summary>
    public class StreamNode : IEquatable<StreamNode>
    {
        public int Id { get; }
        public int Parallelism { get; set; }
        /// <summary>
        /// Maximum parallelism for this stream node.
        /// The maximum parallelism is the upper limit for dynamic scaling and the number of key groups used for partitioned state.
        /// </summary>
        public int MaxParallelism { get; set; }
        public ResourceSpec MinResources = ResourceSpec.Default;
        public ResourceSpec PreferredResources = ResourceSpec.Default;
        public int ManagedMemoryWeight { get; set; } = Transformation<object>.DefaultManagedMemoryWeight;
        public long BufferTimeout { get; set; }
        public IStreamOperator<object> Operator => (OperatorFactory as SimpleOperatorFactory<object>)?.Operator;
        public string OperatorName { get; }
        public string SlotSharingGroup { get; set; }
        public string CoLocationGroup { get; set; }
        public IKeySelector<object, object> StatePartitioner1 { get; set; }
        public IKeySelector<object, object> StatePartitioner2 { get; set; }
        public TypeSerializer<object> StateKeySerializer { get; set; }

        public IStreamOperatorFactory<object> OperatorFactory { get; }
        public IList<IOutputSelector<object>> OutputSelectors { get; }
        public TypeSerializer<object> TypeSerializerIn1 { get; set; }
        public TypeSerializer<object> TypeSerializerIn2 { get; set; }
        public TypeSerializer<object> TypeSerializerOut { get; set; }

        public IList<StreamEdge> InEdges = new List<StreamEdge>();
        public IList<StreamEdge> OutEdges = new List<StreamEdge>();

        public Type JobVertexClass { get; }

        public IInputFormat<object, IInputSplit> InputFormat { get; set; }
        public IOutputFormat<object> OutputFormat { get; set; }

        public string TransformationUid { get; set; }
        public string UserHash { get; set; }

        public StreamNode(
            int id,
            string slotSharingGroup,
            string coLocationGroup,
            IStreamOperator<object> @operator,
            string operatorName,
            List<IOutputSelector<object>> outputSelector,
            Type jobVertexClass)
            : this(
                id,
                slotSharingGroup,
                coLocationGroup,
                SimpleOperatorFactory<object>.Of(@operator),
                operatorName,
                outputSelector,
                jobVertexClass)
        { }

        public StreamNode(
            int id,
            string slotSharingGroup,
            string coLocationGroup,
            IStreamOperatorFactory<object> operatorFactory,
            string operatorName,
            List<IOutputSelector<object>> outputSelector,
            Type jobVertexClass)
        {
            Id = id;
            OperatorName = operatorName;
            OperatorFactory = operatorFactory;
            OutputSelectors = outputSelector;
            JobVertexClass = jobVertexClass;
            SlotSharingGroup = slotSharingGroup;
            CoLocationGroup = coLocationGroup;
        }

        public void AddInEdge(StreamEdge inEdge)
        {
            if (inEdge.TargetId != Id)
            {
                throw new IllegalArgumentException("Destination id doesn't match the StreamNode id");
            }

            InEdges.Add(inEdge);
        }

        public void AddOutEdge(StreamEdge outEdge)
        {
            if (outEdge.SourceId != Id)
            {
                throw new IllegalArgumentException("Source id doesn't match the StreamNode id");
            }

            OutEdges.Add(outEdge);
        }

        public IList<int> OutEdgeIndices => OutEdges.Select(edge => edge.TargetId).ToList();
        public IList<int> InEdgeIndices => InEdges.Select(edge => edge.SourceId).ToList();

        public void SetResources(ResourceSpec minResources, ResourceSpec preferredResources)
        {
            MinResources = minResources;
            PreferredResources = preferredResources;
        }

        public void AddOutputSelector(IOutputSelector<object> outputSelector) => OutputSelectors.Add(outputSelector);

        public bool IsSameSlotSharingGroup(StreamNode downstreamVertex) =>
            (SlotSharingGroup == null && downstreamVertex.SlotSharingGroup == null) ||
            (SlotSharingGroup != null && SlotSharingGroup.Equals(downstreamVertex.SlotSharingGroup));

        public override string ToString() => $"{OperatorName}-{Id}";

        public bool Equals(StreamNode other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Id == other.Id;
        }

        public override bool Equals(object obj) => obj is StreamNode other && Equals(other);

        public override int GetHashCode() => Id;
    }
}
