using System;
using System.Collections.Generic;

namespace FLink.Streaming.Runtime.StreamRecord
{
    /// <summary>
    /// One value in a data stream. This stores the value and an optional associated timestamp.
    /// </summary>
    /// <typeparam name="T">The type encapsulated with the stream record.</typeparam>
    public class StreamRecord<T> : StreamElement, IEquatable<StreamRecord<T>>
    {
        /// <summary>
        /// The actual value held by this record.
        /// </summary>
        public T Value;
        /// <summary>
        /// The timestamp of the record.
        /// </summary>
        public long Timestamp;
        /// <summary>
        /// Flag whether the timestamp is actually set.
        /// </summary>
        public bool HasTimestamp;

        /// <summary>
        /// Creates a new StreamRecord. The record does not have a timestamp.
        /// </summary>
        /// <param name="value">The value to wrap</param>
        public StreamRecord(T value) => Value = value;

        /// <summary>
        /// Creates a new StreamRecord wrapping the given value. The timestamp is set to the given timestamp.
        /// </summary>
        /// <param name="value">The value to wrap</param>
        /// <param name="timestamp">The timestamp in milliseconds</param>
        public StreamRecord(T value, long timestamp)
        {
            Value = value;
            Timestamp = timestamp;
            HasTimestamp = true;
        }

        #region [ Updating ] 

        public StreamRecord<T> Replace(T element)
        {
            Value = element;

            return this;
        }

        public StreamRecord<T> Replace(T value, long timestamp)
        {
            Timestamp = timestamp;
            Value = value;
            HasTimestamp = true;

            return this;
        }

        #endregion

        #region [ Utilities ]

        public bool Equals(StreamRecord<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(Value, other.Value) && Timestamp == other.Timestamp && HasTimestamp == other.HasTimestamp;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StreamRecord<T>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<T>.Default.GetHashCode(Value);
                hashCode = (hashCode * 397) ^ Timestamp.GetHashCode();
                hashCode = (hashCode * 397) ^ HasTimestamp.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString() => "Record @ " + (HasTimestamp ? $"{Timestamp}" : "(undef)") + " : " + Value;

        #endregion
    }
}
