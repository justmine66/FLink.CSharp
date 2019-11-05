namespace FLink.Metrics.Core
{
    /// <summary>
    /// Metric for measuring throughput.
    /// </summary>
    public interface IMeter
    {
        /// <summary>
        /// Mark occurrence of an event.
        /// </summary>
        void MarkEvent();

        /// <summary>
        /// Mark occurrence of multiple events.
        /// </summary>
        /// <param name="n">number of events occurred</param>
        void MarkEvent(long n);

        /// <summary>
        /// Get the current rate of events per second.
        /// </summary>
        double Rate { get; }

        /// <summary>
        /// Get number of events marked on the meter.
        /// </summary>
        long Count { get; }
    }
}
