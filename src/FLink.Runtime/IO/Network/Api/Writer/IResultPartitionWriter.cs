using System;
using FLink.Core.IO;
using FLink.Runtime.IO.Network.Buffers;
using FLink.Runtime.IO.Network.Partition;

namespace FLink.Runtime.IO.Network.Api.Writer
{
    /// <summary>
    /// A buffer-oriented runtime result writer API for producing results.
    /// </summary>
    public interface IResultPartitionWriter : ICloseable
    {
        /// <summary>
        /// Setup partition, potentially heavy-weight, blocking operation comparing to just creation.
        /// </summary>
        void Setup();

        ResultPartitionId PartitionId { get; }

        int NumberOfSubpartitions { get; }

        int NumTargetKeyGroups { get; }

        /// <summary>
        /// Requests a <see cref="BufferBuilder"/> from this partition for writing data.
        /// </summary>
        BufferBuilder BufferBuilder { get; }

        /// <summary>
        /// Adds the bufferConsumer to the subpartition with the given index.
        /// This method takes the ownership of the passed <see cref="BufferConsumer"/> and thus is responsible for releasing it's resources.
        /// To avoid problems with data re-ordering, before adding new <see cref="BufferConsumer"/> the previously added one the given <paramref name="subpartitionIndex"/> must be marked as <see cref="BufferConsumer.IsFinished"/>.
        /// </summary>
        /// <param name="bufferConsumer"></param>
        /// <param name="subpartitionIndex"></param>
        /// <returns>true if operation succeeded and bufferConsumer was enqueued for consumption.</returns>
        bool AddBufferConsumer(BufferConsumer bufferConsumer, int subpartitionIndex);

        /// <summary>
        /// Manually trigger consumption from enqueued <see cref="BufferConsumer"/>s in all subpartitions.
        /// </summary>
        void FlushAll();

        /// <summary>
        /// Manually trigger consumption from enqueued <see cref="BufferConsumer"/>s in one specified subpartition.
        /// </summary>
        /// <param name="subpartitionIndex"></param>
        void Flush(int subpartitionIndex);

        /// <summary>
        /// Fail the production of the partition.
        /// This method propagates non-null failure causes to consumers on a best-effort basis. This call also leads to the release of all resources associated with the partition. Closing of the partition is still needed afterwards if it has not been done before.
        /// </summary>
        /// <param name="exception">throwable failure cause</param>
        void Fail(Exception exception = null);

        /// <summary>
        /// Successfully finish the production of the partition.
        /// Closing of partition is still needed afterwards.
        /// </summary>
        void Finish();
    }
}
