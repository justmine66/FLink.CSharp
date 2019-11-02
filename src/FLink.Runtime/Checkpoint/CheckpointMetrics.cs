using FLink.Core.Util;

namespace FLink.Runtime.Checkpoint
{
    /// <summary>
    /// A collection of simple metrics, around the triggering of a checkpoint.
    /// </summary>
    public class CheckpointMetrics
    {
        /// <summary>
        /// The number of bytes that were buffered during the checkpoint alignment phase
        /// </summary>
        public long BytesBufferedInAlignment;

        /// <summary>
        /// The duration (in nanoseconds) that the stream alignment for the checkpoint took
        /// </summary>
        public long AlignmentDurationNanos;

        /// <summary>
        /// The duration (in milliseconds) of the synchronous part of the operator checkpoint
        /// </summary>
        public long SyncDurationMillis;

        /// <summary>
        /// The duration (in milliseconds) of the asynchronous part of the operator checkpoint
        /// </summary>
        public long AsyncDurationMillis;

        public CheckpointMetrics()
            : this(-1L, -1L, -1L, -1L)
        {
        }

        public CheckpointMetrics(long bytesBufferedInAlignment, long alignmentDurationNanos, long syncDurationMillis, long asyncDurationMillis)
        {
            Preconditions.CheckArgument(syncDurationMillis >= -1);
            Preconditions.CheckArgument(asyncDurationMillis >= -1);
            Preconditions.CheckArgument(bytesBufferedInAlignment >= -1);
            Preconditions.CheckArgument(alignmentDurationNanos >= -1);

            BytesBufferedInAlignment = bytesBufferedInAlignment;
            AlignmentDurationNanos = alignmentDurationNanos;
            SyncDurationMillis = syncDurationMillis;
            AsyncDurationMillis = asyncDurationMillis;
        }
    }
}
