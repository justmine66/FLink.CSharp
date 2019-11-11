using System;
using FLink.Core.Api.Common.Operators;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Api.CSharp;

namespace FLink.Streaming.Api.Graph
{
    /// <summary>
    /// Representing the operators in the streaming programs, with all their properties.
    /// </summary>
    [Serializable]
    public class StreamNode
    {
        public int Id { get; internal set; }
        public int Parallelism { get; internal set; }
        /// <summary>
        /// Maximum parallelism for this stream node.
        /// The maximum parallelism is the upper limit for dynamic scaling and the number of key groups used for partitioned state.
        /// </summary>
        public int MaxParallelism { get; internal set; }
        public ResourceSpec MinResources = ResourceSpec.Default;
        public ResourceSpec PreferredResources = ResourceSpec.Default;
        public long BufferTimeout;
        public string OperatorName;
        public string SlotSharingGroup;
        public string CoLocationGroup;
        public IKeySelector<object, object> StatePartitioner1;
        public IKeySelector<object, object> StatePartitioner2;
        public TypeSerializer<object> StateKeySerializer;
    }
}
