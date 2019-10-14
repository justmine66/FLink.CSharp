namespace FLink.Streaming.Runtime.Operators.Windowing
{
    /// <summary>
    /// Stores the value and the timestamp of the record.
    /// </summary>
    /// <typeparam name="T">The type encapsulated value</typeparam>
    public class TimestampedValue<T>
    {
        public T Value { get; }
        public long Timestamp { get; }
        public bool HasTimestamp { get; }

        public TimestampedValue(T value)
        {
            Value = value;
        }

        public TimestampedValue(T value, long timestamp)
        {
            Value = value;
            Timestamp = timestamp;
            HasTimestamp = true;
        }
    }
}
