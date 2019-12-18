using System.Collections.Generic;
using FLink.Core.Util;
using FLink.Streaming.Api.Transformations;
using FLink.Streaming.Runtime.Partitioners;

namespace FLink.Streaming.Api.Graph
{
    /// <summary>
    /// An edge in the streaming topology.
    /// One edge like this does not necessarily gets converted to a connection between two job vertices(due to chaining/optimization).
    /// </summary>
    public class StreamEdge
    {
        public string EdgeId;
        public int SourceId;
        public int TargetId;

        /// <summary>
        /// The type number of the input for co-tasks.
        /// </summary>
        public int TypeNumber;

        /// <summary>
        /// A list of output names that the target vertex listens to (if there is output selection).
        /// </summary>
        public IList<string> SelectedNames;

        /// <summary>
        /// The side-output tag (if any) of this <see cref="StreamEdge"/>.
        /// </summary>
        public OutputTag<object> OutputTag;

        /// <summary>
        /// The <see cref="StreamPartitioner{T}"/> on this <see cref="StreamEdge"/>.
        /// </summary>
        private StreamPartitioner<object> OutputPartitioner;

        /// <summary>
        /// The name of the operator in the source vertex.
        /// </summary>
        public string SourceOperatorName;

        /// <summary>
        /// The name of the operator in the target vertex.
        /// </summary>
        public string TargetOperatorName;

        public ShuffleMode ShuffleMode;

        public StreamEdge(StreamNode sourceVertex, StreamNode targetVertex, int typeNumber,
            List<string> selectedNames, StreamPartitioner<object> outputPartitioner, OutputTag<object> outputTag)
            : this(sourceVertex,
                targetVertex,
                typeNumber,
                selectedNames,
                outputPartitioner,
                outputTag,
                ShuffleMode.Undefined)
        { }

        public StreamEdge(StreamNode sourceVertex, StreamNode targetVertex, int typeNumber,
            IList<string> selectedNames, StreamPartitioner<object> outputPartitioner, OutputTag<object> outputTag,
            ShuffleMode shuffleMode)
        {
            SourceId = sourceVertex.Id;
            TargetId = targetVertex.Id;
            TypeNumber = typeNumber;
            SelectedNames = selectedNames;
            OutputPartitioner = outputPartitioner;
            OutputTag = outputTag;
            SourceOperatorName = sourceVertex.OperatorName;
            TargetOperatorName = targetVertex.OperatorName;
            ShuffleMode = Preconditions.CheckNotNull(shuffleMode);

            EdgeId = sourceVertex + "_" + targetVertex + "_" + typeNumber + "_" + selectedNames + "_" + outputPartitioner;
        }
    }
}
