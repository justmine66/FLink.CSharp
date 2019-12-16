using System;
using FLink.Core.Exceptions;
using FLink.Core.Memory;
using FLink.Runtime.Checkpoint;
using FLink.Runtime.Events;

namespace FLink.Runtime.IO.Network.Api
{
    /// <summary>
    /// Checkpoint barriers are used to align checkpoints throughout the streaming topology.
    /// the barriers are emitted by the sources when instructed to do so by the JobManager. When operators receive a CheckpointBarrier on one of its inputs, it knows that this is the point between the pre-checkpoint and post-checkpoint data.
    /// Once an operator has received a checkpoint barrier from all its input channels, it knows that a certain checkpoint is complete. It can trigger the operator specific checkpoint behavior and broadcast the barrier to downstream operators.
    /// Depending on the semantic guarantees, may hold off post-checkpoint data until the checkpoint is complete (exactly once).
    /// The checkpoint barrier IDs are strictly monotonous increasing.
    /// </summary>
    public class CheckpointBarrier : RuntimeEvent, IEquatable<CheckpointBarrier>
    {
        public CheckpointBarrier(long id, long timestamp, CheckpointOptions checkpointOptions)
        {
            Id = id;
            Timestamp = timestamp;
            CheckpointOptions = checkpointOptions;
        }

        public long Id { get; }
        public long Timestamp { get; }
        public CheckpointOptions CheckpointOptions { get; }

        public override void Write(IDataOutputView output)
            => throw new UnSupportedOperationException("This method should never be called");

        public override void Read(IDataInputView input)
            => throw new UnSupportedOperationException("This method should never be called");

        public bool Equals(CheckpointBarrier other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Id == other.Id && Timestamp == other.Timestamp;
        }

        public override bool Equals(object obj) => obj is CheckpointBarrier other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var uId = (uint) Id;
                var uTimestamp = (uint)Timestamp;

                return (int)(Id ^ (uId >> 32) ^ Timestamp ^ (uTimestamp >> 32));
            }
        }
    }
}
