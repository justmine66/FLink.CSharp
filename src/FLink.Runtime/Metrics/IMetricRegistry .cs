using FLink.Metrics.Core;
using FLink.Runtime.Metrics.Dump;
using FLink.Runtime.Metrics.Groups;
using FLink.Runtime.Metrics.Scope;

namespace FLink.Runtime.Metrics
{
    /// <summary>
    /// Interface for a metric registry.
    /// </summary>
    public interface IMetricRegistry
    {
        /// <summary>
        /// Returns the global delimiter.
        /// </summary>
        char Delimiter { get; }

        /// <summary>
        /// Returns the configured delimiter for the reporter with the given index.
        /// </summary>
        /// <param name="index">of the reporter whose delimiter should be used</param>
        /// <returns>configured reporter delimiter, or global delimiter if index is invalid</returns>
        char GetDelimiter(int index);

        /// <summary>
        /// Returns the number of registered reporters.
        /// </summary>
        int NumberReporters { get; }

        /// <summary>
        /// Registers a new <see cref="IMetric"/> with this registry.
        /// </summary>
        /// <param name="metric">the metric that was added</param>
        /// <param name="metricName">the name of the metric</param>
        /// <param name="group">the group that contains the metric</param>
        void Register<T>(IMetric metric, string metricName, AbstractMetricGroup<T> group);

        /// <summary>
        /// Un-Registers a new <see cref="IMetric"/> with this registry.
        /// </summary>
        /// <param name="metric">the metric that was added</param>
        /// <param name="metricName">the name of the metric</param>
        /// <param name="group">the group that contains the metric</param>
        void UnRegister<T>(IMetric metric, string metricName, AbstractMetricGroup<T> group);

        /// <summary>
        /// Returns the scope formats.
        /// </summary>
        ScopeFormats ScopeFormats { get; }

        /// <summary>
        /// Returns the path of the <see cref="IMetricQueryService"/> or null, if none is started.
        /// </summary>
        string MetricQueryServicePath { get; }
    }
}
