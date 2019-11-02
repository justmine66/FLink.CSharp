using System;

namespace FLink.Runtime.Checkpoint
{
    /// <summary>
    /// Encapsulates all the meta data for a checkpoint.
    /// </summary>
    public class CheckpointMetaData : IEquatable<CheckpointMetaData>
    {
        /// <summary>
        /// The ID of the checkpoint
        /// </summary>
        public long CheckpointId;

        /// <summary>
        /// The timestamp of the checkpoint
        /// </summary>
        public long Timestamp;

        public CheckpointMetaData(long checkpointId, long timestamp)
        {
            CheckpointId = checkpointId;
            Timestamp = timestamp;
        }

        public bool Equals(CheckpointMetaData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return CheckpointId == other.CheckpointId && Timestamp == other.Timestamp;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CheckpointMetaData)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (CheckpointId.GetHashCode() * 397) ^ Timestamp.GetHashCode();
            }
        }

        public override string ToString() => "CheckpointMetaData{" +
                                            "checkpointId=" + CheckpointId +
                                            ", timestamp=" + Timestamp +
                                            '}';
    }
}
