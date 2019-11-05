namespace FLink.Metrics.Core
{
    /// <summary>
    /// Histogram statistics represent the current snapshot of elements recorded in the histogram.
    /// The histogram statistics allow to calculate values for quantiles, the mean, the standard deviation, the minimum and the maximum.
    /// </summary>
    public abstract class HistogramStatistics
    {
        /// <summary>
        /// Returns the value for the given quantile based on the represented histogram statistics.
        /// </summary>
        /// <param name="quantile">to calculate the value for</param>
        /// <returns>for the given quantile</returns>
        public abstract double GetQuantile(double quantile);

        /// <summary>
        /// Returns the elements of the statistics' sample.
        /// </summary>
        public abstract long[] Values { get; }

        /// <summary>
        /// Returns the size of the statistics' sample.
        /// </summary>
        public abstract int Size { get; }

        /// <summary>
        /// Returns the mean of the histogram values.
        /// </summary>
        public abstract double Mean { get; }

        /// <summary>
        /// Returns the standard deviation of the distribution reflected by the histogram statistics.
        /// </summary>
        public abstract double StandardDeviation { get; }

        /// <summary>
        /// Returns the minimum value of the histogram.
        /// </summary>
        public abstract double Minimum { get; }

        /// <summary>
        /// Returns the maximum value of the histogram.
        /// </summary>
        public abstract double Maximum { get; }
    }
}
