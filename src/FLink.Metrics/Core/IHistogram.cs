namespace FLink.Metrics.Core
{
    /// <summary>
    /// Histogram interface to be used with Flink's metrics system.
    /// The histogram allows to record values, get the current count of recorded values and create histogram statistics for the currently seen elements.
    /// </summary>
    public interface IHistogram
    {
        /// <summary>
        /// Update the histogram with the given value.
        /// </summary>
        /// <param name="value">to update the histogram with</param>
        void Update(long value);

        /// <summary>
        /// Get the count of seen elements.
        /// </summary>
        long Count { get; }

        /// <summary>
        /// Create statistics for the currently recorded elements.
        /// </summary>
        HistogramStatistics Statistics { get; }
    }
}
