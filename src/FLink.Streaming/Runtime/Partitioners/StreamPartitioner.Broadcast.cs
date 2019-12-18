using FLink.Core.Exceptions;
using FLink.Runtime.Pluggable;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Runtime.Partitioners
{
    /// <summary>
    /// Partitioner that selects all the output channels.
    /// </summary>
    /// <typeparam name="TElement">Type of the elements in the Stream being broadcast</typeparam>
    public class BroadcastPartitioner<TElement> : StreamPartitioner<TElement>
    {
        public override int SelectChannel(SerializationDelegate<StreamRecord<TElement>> record)
        {
            throw new UnSupportedOperationException("Broadcast partitioner does not support select channels.");
        }

        public override StreamPartitioner<TElement> Copy() => this;

        public override bool IsBroadcast => true;

        public override string ToString() => "BROADCAST";
    }
}
