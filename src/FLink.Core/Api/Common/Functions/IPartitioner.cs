namespace FLink.Core.Api.Common.Functions
{
    /// <summary>
    /// Function to implement a custom partition assignment for keys.
    /// </summary>
    /// <typeparam name="TKey">The type of the key to be partitioned.</typeparam>
    public interface IPartitioner<in TKey> : IFunction
    {
        /// <summary>
        /// Computes the partition for the given key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="numPartitions">The number of partitions to partition into.</param>
        /// <returns>The partition index.</returns>
        int Partition(TKey key, int numPartitions);
    }
}
