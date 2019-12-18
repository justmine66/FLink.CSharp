using System;
using FLink.Runtime.Pluggable;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Runtime.Partitioners
{
    /// <summary>
    /// Partitioner that distributes the data equally by cycling through the output channels.
    /// </summary>
    /// <typeparam name="TElement">Type of the elements in the Stream being rescaled</typeparam>
    public class RescalePartitioner<TElement> : StreamPartitioner<TElement>
    {
        private int _nextChannelToSendTo = -1;

        public override int SelectChannel(SerializationDelegate<StreamRecord<TElement>> record)
        {
            if (++_nextChannelToSendTo >= NumberOfChannels)
            {
                _nextChannelToSendTo = 0;
            }

            return _nextChannelToSendTo;
        }

        public override StreamPartitioner<TElement> Copy() => this;

        public override string ToString() => "RESCALE";
    }
}
