using System;
using FLink.Core.Api.CSharp.Functions;
using FLink.Core.Exceptions;
using FLink.Core.Util;
using FLink.Runtime.Pluggable;
using FLink.Runtime.State;
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

        public int MaxParallelism { get; private set; }

        public KeyGroupStreamPartitioner(IKeySelector<TElement, TKey> keySelector, int maxParallelism)
        {
            Preconditions.CheckArgument(maxParallelism > 0, "Number of key-groups must be > 0!");
            KeySelector = Preconditions.CheckNotNull(keySelector);
            MaxParallelism = maxParallelism;
        }

        /// <summary>
        /// KeyBy()算子底层所采用的StreamPartitioner.
        /// </summary>
        /// <param name="record">the stream record.</param>
        /// <returns>the sub-task id.</returns>
        public override int SelectChannel(SerializationDelegate<StreamRecord<TElement>> record)
        {
            TKey key;
            try
            {
                key = KeySelector.GetKey(record.Instance.Value);
            }
            catch (Exception e)
            {
                throw new RuntimeException($"Could not extract key from {record.Instance.Value}", e);
            }

            return KeyGroupRangeAssignment.AssignKeyToParallelOperator(key, MaxParallelism, NumberOfChannels);
        }

        public override StreamPartitioner<TElement> Copy() => this;

        public void Configure(int maxParallelism)
        {
            KeyGroupRangeAssignment.CheckParallelismPreconditions(maxParallelism);
            MaxParallelism = maxParallelism;
        }

        public override string ToString() => "HASH";
    }
}
