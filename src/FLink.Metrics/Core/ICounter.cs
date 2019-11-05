namespace FLink.Metrics.Core
{
    /// <summary>
    /// A Counter is a <see cref="IMetric"/> that measures a count.
    /// </summary>
    public interface ICounter
    {
        /// <summary>
        /// Increment the current count by 1.
        /// </summary>
        void Increment();

        /// <summary>
        /// Increment the current count by the given value.
        /// </summary>
        /// <param name="value">to increment the current count by</param>
        void Increment(long value);

        /// <summary>
        /// Decrement the current count by 1.
        /// </summary>
        void Decrement();

        /// <summary>
        /// Decrement the current count by the given value.
        /// </summary>
        /// <param name="value">to decrement the current count by</param>
        void Decrement(long value);

        /// <summary>
        /// The current count.
        /// </summary>
        long Count { get; }
    }
}
