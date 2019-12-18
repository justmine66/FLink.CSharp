using System;
using System.Threading;
using FLink.Runtime.Pluggable;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Runtime.Partitioners
{
    /// <summary>
    /// Partitioner that distributes the data equally by cycling through the output.
    /// </summary>
    /// <typeparam name="TElement">Type of the elements in the Stream being rebalanced</typeparam>
    public class RebalancePartitioner<TElement> : StreamPartitioner<TElement>
    {
        private readonly ThreadLocal<Random> _threadLocalRandom = new ThreadLocal<Random>();

        private int _nextChannelToSendTo;

        public override void Setup(int numberOfChannels)
        {
            base.Setup(numberOfChannels);

            _nextChannelToSendTo = _threadLocalRandom.Value.Next(numberOfChannels);
        }

        public override int SelectChannel(SerializationDelegate<StreamRecord<TElement>> record)
            => (_nextChannelToSendTo + 1) % NumberOfChannels;

        public override StreamPartitioner<TElement> Copy() => this;

        public override string ToString() => "REBALANCE";
    }
}
