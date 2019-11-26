using FLink.Core.Api.CSharp.Functions;
using FLink.Core.Util;
using FLink.Runtime.Pluggable;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Runtime.Partitioners
{
    /// <summary>
    /// Partitioner selects the target channel based on the key group index.
    /// </summary>
    /// <typeparam name="TElement">Type of the elements in the Stream being partitioned</typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class KeyGroupStreamPartitioner<TElement, TKey> : StreamPartitioner<TElement>, IConfigurableStreamPartitioner
    {
        public IKeySelector<TElement, TKey> KeySelector { get; }

        public int MaxParallelism { get; }

        public KeyGroupStreamPartitioner(IKeySelector<TElement, TKey> keySelector, int maxParallelism)
        {
            Preconditions.CheckArgument(maxParallelism > 0, "Number of key-groups must be > 0!");
            KeySelector = Preconditions.CheckNotNull(keySelector);
            MaxParallelism = maxParallelism;
        }

        public override int SelectChannel(SerializationDelegate<StreamRecord<TElement>> record)
        {
            throw new System.NotImplementedException();
        }

        public override StreamPartitioner<TElement> Copy() => this;

        public void Configure(int maxParallelism)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString() => "HASH";
    }
}
