using System;
using FLink.Core.Exceptions;
using FLink.Core.Memory;
using FLink.Runtime.Events;

namespace FLink.Runtime.IO.Network.Api
{
    /// <summary>
    /// The CancelCheckpointMarker travels through the data streams, similar to the <see cref="CheckpointBarrier"/>, but signals that a certain checkpoint should be canceled. Any in-progress alignment for that checkpoint needs to be canceled and regular processing should be resumed.
    /// </summary>
    public class CancelCheckpointMarker : RuntimeEvent, IEquatable<CancelCheckpointMarker>
    {
        public CancelCheckpointMarker(long checkpointId) => CheckpointId = checkpointId;

        /// <summary>
        /// The id of the checkpoint to be canceled.
        /// </summary>
        public long CheckpointId { get; }

        #region [ These known and common event go through special code paths, rather than through generic serialization. ]

        public override void Write(IDataOutputView output)
            => throw new UnSupportedOperationException("this method should never be called");

        public override void Read(IDataInputView input)
            => throw new UnSupportedOperationException("this method should never be called");

        #endregion

        public bool Equals(CancelCheckpointMarker other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return CheckpointId == other.CheckpointId;
        }

        public override bool Equals(object obj) => obj is CancelCheckpointMarker other && Equals(other);

        public override int GetHashCode() => (int) (CheckpointId ^ (CheckpointId >> 32));

        public override string ToString() => $"CancelCheckpointMarker {CheckpointId}";
    }
}
