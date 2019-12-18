using System;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.CSharp.Functions;
using FLink.Core.Exceptions;
using FLink.Runtime.Pluggable;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Runtime.Partitioners
{
    /// <summary>
    /// Partitioner that selects the channel with a user defined partitioner function on a key.
    /// </summary>
    /// <typeparam name="TKey">Type of the key</typeparam>
    /// <typeparam name="TData">Type of the data</typeparam>
    public class CustomPartitionerWrapper<TKey, TData> : StreamPartitioner<TData>
    {
        private readonly IPartitioner<TKey> partitioner;
        private readonly IKeySelector<TData, TKey> keySelector;

        public CustomPartitionerWrapper(IPartitioner<TKey> partitioner, IKeySelector<TData, TKey> keySelector)
        {
            this.partitioner = partitioner;
            this.keySelector = keySelector;
        }

        public override int SelectChannel(SerializationDelegate<StreamRecord<TData>> record)
        {
            TKey key;
            try
            {
                key = keySelector.GetKey(record.Instance.Value);
            }
            catch (Exception e)
            {
                throw new RuntimeException($"Could not extract key from {record.Instance}", e);
            }

            return partitioner.Partition(key, NumberOfChannels);
        }

        public override StreamPartitioner<TData> Copy() => this;

        public override string ToString() => "CUSTOM";
    }
}
