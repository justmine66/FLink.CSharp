using FLink.Runtime.IO.Network.Api.Writer;
using FLink.Runtime.Pluggable;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Runtime.Partitioners
{
    /// <summary>
    /// A special <see cref="IChannelSelector{TRecord}"/> for use in streaming programs.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public abstract class StreamPartitioner<TElement> : IChannelSelector<SerializationDelegate<StreamRecord<TElement>>>
    {
        public int NumberOfChannels { get; private set; }

        public virtual void Setup(int numberOfChannels) => NumberOfChannels = numberOfChannels;

        public abstract int SelectChannel(SerializationDelegate<StreamRecord<TElement>> record);

        public virtual bool IsBroadcast => false;

        public abstract StreamPartitioner<TElement> Copy();
    }
}
