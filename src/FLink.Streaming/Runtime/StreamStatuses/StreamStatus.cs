using System;
using FLink.Core.Exceptions;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Runtime.StreamStatuses
{
    /// <summary>
    /// A Stream Status element informs stream tasks whether or not they should continue to expect records and watermarks from the input stream that sent them.
    /// </summary>
    public class StreamStatus : StreamElement, IEquatable<StreamStatus>
    {
        public static readonly int IdleStatus = -1;
        public static readonly int ActiveStatus = 0;

        public static readonly StreamStatus Idle = new StreamStatus(IdleStatus);
        public static readonly StreamStatus Active = new StreamStatus(ActiveStatus);

        public int Status;

        public StreamStatus(int status)
        {
            if (status != IdleStatus && status != ActiveStatus)
            {
                throw new IllegalArgumentException("Invalid status value for StreamStatus; " +
                                                   "allowed values are " + ActiveStatus + " (for ACTIVE) and " + IdleStatus + " (for IDLE).");
            }

            Status = status;
        }

        public bool IsIdle => Status == IdleStatus;

        public bool IsActive => !IsIdle;

        public bool Equals(StreamStatus other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Status == other.Status;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StreamStatus)obj);
        }

        public override int GetHashCode() => Status;

        public override string ToString()
        {
            var statusStr = (Status == ActiveStatus) ? "ACTIVE" : "IDLE";
            return "StreamStatus(" + statusStr + ")";
        }
    }
}
