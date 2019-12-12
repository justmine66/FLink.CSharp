namespace FLink.Runtime.IO.Network.Partition
{
    /// <summary>
    /// Type of a result partition.
    /// </summary>
    public class ResultPartitionType
    {
        /// <summary>
        /// Blocking partitions represent blocking data exchanges, where the data stream is first fully produced and then consumed.
        /// This is an option that is only applicable to bounded streams and can be used in bounded stream runtime and recovery.
        /// Blocking partitions can be consumed multiple times and concurrently.
        /// The partition is not automatically released after being consumed(like for example the <see cref="Pipelined"/> partitions), but only released through the scheduler, when it determines that the partition is no longer needed.
        /// </summary>
        public static readonly ResultPartitionType Blocking = new ResultPartitionType(false, false, false, false);

        /// <summary>
        /// BlockingPersistent partitions are similar to <see cref="Blocking"/> , but have a user-specified life cycle.
        /// BlockingPersistent partitions are dropped upon explicit API calls to the JobManager or ResourceManager, rather than by the scheduler.
        /// Otherwise, the partition may only be dropped by safety-nets during failure handling scenarios, like when the TaskManager exits or when the TaskManager looses connection to JobManager / ResourceManager for too long.
        /// </summary>
        public static readonly ResultPartitionType BlockingPersistent = new ResultPartitionType(false, false, false, true);

        /// <summary>
        /// A pipelined streaming data exchange. This is applicable to both bounded and unbounded streams.
        /// Pipelined results can be consumed only once by a single consumer and are automatically disposed when the stream has been consumed.
        /// This result partition type may keep an arbitrary amount of data in-flight, in contrast to the <see cref="PipelinedBounded"/> variant.
        /// </summary>
        public static readonly ResultPartitionType Pipelined = new ResultPartitionType(true, true, false, false);

        /// <summary>
        /// Pipelined partitions with a bounded (local) buffer pool.
        /// For streaming jobs, a fixed limit on the buffer pool size should help avoid that too much data is being buffered and checkpoint barriers are delayed. In contrast to limiting the overall network buffer pool size, this, however, still allows to be flexible with regards to the total number of partitions by selecting an appropriately big network buffer pool size.
        /// For batch jobs, it will be best to keep this unlimited (<see cref="Pipelined"/>) since there are no checkpoint barriers.
        /// </summary>
        public static readonly ResultPartitionType PipelinedBounded = new ResultPartitionType(true, true, true, false);

        /// <summary>
        /// Specifies the behaviour of an intermediate result partition at runtime.
        /// </summary>
        /// <param name="isPipelined"></param>
        /// <param name="hasBackPressure"></param>
        /// <param name="isBounded"></param>
        /// <param name="isPersistent"></param>
        public ResultPartitionType(bool isPipelined, bool hasBackPressure, bool isBounded, bool isPersistent)
        {
            IsPipelined = isPipelined;
            HasBackPressure = hasBackPressure;
            IsBounded = isBounded;
            IsPersistent = isPersistent;
        }

        /// <summary>
        /// Can the partition be consumed while being produced?
        /// </summary>
        public bool IsPipelined { get; }

        /// <summary>
        /// Does the partition produce back pressure when not consumed?
        /// </summary>
        public bool HasBackPressure { get; }

        /// <summary>
        /// Does this partition use a limited number of (network) buffers?
        /// </summary>
        public bool IsBounded { get; }

        /// <summary>
        /// This partition will not be released after consuming if 'isPersistent' is true.
        /// </summary>
        public bool IsPersistent { get; }
    }
}
