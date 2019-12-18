using System;
using FLink.Runtime.Pluggable;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Runtime.Partitioners
{
    /// <summary>
    /// Partitioner that distributes the data equally by selecting one output channel randomly.
    /// </summary>
    /// <typeparam name="TElement">Type of the Tuple</typeparam>
    public class ShufflePartitioner<TElement> : StreamPartitioner<TElement>
    {
        private Random _random = new Random();

        public override int SelectChannel(SerializationDelegate<StreamRecord<TElement>> record) => _random.Next(NumberOfChannels);

        public override StreamPartitioner<TElement> Copy() => new ShufflePartitioner<TElement>();

        public override string ToString() => "SHUFFLE";
    }
}
