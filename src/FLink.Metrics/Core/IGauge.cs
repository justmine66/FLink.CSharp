namespace FLink.Metrics.Core
{
    /// <summary>
    /// A Gauge is a <see cref="IMetric"/> that calculates a specific value at a point in time.
    /// </summary>
    public interface IGauge<out TValue> : IMetric
    {
        /// <summary>
        /// Calculates and returns the measured value.
        /// </summary>
        TValue Value { get; }
    }
}
