using System;
using FLink.Runtime.JobGraphs;

namespace FLink.Streaming.Runtime.StreamRecord
{
    /// <summary>
    /// Special record type carrying a timestamp of its creation time at a source operator and the vertexId and subtask index of the operator.
    /// At sinks, the marker can be used to approximate the time a record needs to travel through the dataflow.
    /// </summary>
    public class LatencyMarker : StreamElement, IEquatable<LatencyMarker>
    {
        /// <summary>
        /// Gets the timestamp marked by the LatencyMarker.
        /// </summary>
        public long MarkedTime;

        public OperatorId OperatorId;

        public int SubTaskIndex;

        /// <summary>
        /// Creates a latency mark with the given timestamp.
        /// </summary>
        /// <param name="markedTime"></param>
        /// <param name="operatorId"></param>
        /// <param name="subTaskIndex"></param>
        public LatencyMarker(long markedTime, OperatorId operatorId, int subTaskIndex)
        {
            MarkedTime = markedTime;
            OperatorId = operatorId;
            SubTaskIndex = subTaskIndex;
        }

        public bool Equals(LatencyMarker other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return MarkedTime == other.MarkedTime && Equals(OperatorId, other.OperatorId) && SubTaskIndex == other.SubTaskIndex;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LatencyMarker)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = MarkedTime.GetHashCode();
                hashCode = (hashCode * 397) ^ (OperatorId != null ? OperatorId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ SubTaskIndex;
                return hashCode;
            }
        }

        public override string ToString() => "LatencyMarker{" +
                                             "MarkedTime=" + MarkedTime +
                                             ", OperatorId=" + OperatorId +
                                             ", SubTaskIndex=" + SubTaskIndex +
                                             '}';
    }
}
