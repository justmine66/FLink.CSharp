using FLink.Runtime.IO.Network.Api.Writer;
using FLink.Runtime.Plugable;
using FLink.Streaming.Runtime.StreamRecord;

namespace FLink.Streaming.Runtime.Partitioners
{
    /// <summary>
    /// A special <see cref="IChannelSelector{TRecord}"/> for use in streaming programs.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class StreamPartitioner<T> : IChannelSelector<SerializationDelegate<StreamRecord<T>>>
    {
        public int NumberOfChannels;

        public void Setup(int numberOfChannels) => NumberOfChannels = numberOfChannels;

        public abstract int SelectChannel(SerializationDelegate<StreamRecord<T>> record);

        public bool IsBroadcast => false;

        public abstract StreamPartitioner<T> Copy();
    }
}
