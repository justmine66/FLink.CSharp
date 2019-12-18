using FLink.Runtime.Pluggable;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Runtime.Partitioners
{
    /// <summary>
    /// Partitioner that forwards elements only to the locally running downstream operation.
    /// </summary>
    /// <typeparam name="TElement">Type of the elements in the Stream</typeparam>
    public class ForwardPartitioner<TElement> : StreamPartitioner<TElement>
    {
        public override int SelectChannel(SerializationDelegate<StreamRecord<TElement>> record) => 0;

        public override StreamPartitioner<TElement> Copy() => this;

        public override string ToString() => "FORWARD";
    }
}
