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

        /// <summary>
        /// 先随机选择一个下游算子的实例，然后用轮询（round-robin）的方式从该实例开始循环输出。
        /// 该方式能保证完全的下游负载均衡，所以常用来处理有倾斜的原数据流。
        /// </summary>
        /// <param name="record">the stream record.</param>
        /// <returns>the sub-task id.</returns>
        public override int SelectChannel(SerializationDelegate<StreamRecord<TElement>> record)
            => (_nextChannelToSendTo + 1) % NumberOfChannels;

        public override StreamPartitioner<TElement> Copy() => this;

        public override string ToString() => "REBALANCE";
    }
}
