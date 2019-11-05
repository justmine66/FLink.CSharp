using System.Collections.Generic;

namespace FLink.Metrics.Core
{
    /// <summary>
    /// A MetricGroup is a named container and further metric subgroups.
    /// Instances of this class can be used to register new metrics with Flink and to create a nested hierarchy based on the group names.
    /// </summary>
    public interface IMetricGroup
    {
        #region [ Metrics ]

        /// <summary>
        /// Creates and registers a new <see cref="ICounter"/> with Flink.
        /// </summary>
        /// <param name="name">name of the counter</param>
        /// <returns>the created counter</returns>
        ICounter Counter(int name);

        /// <summary>
        /// Creates and registers a new <see cref="ICounter"/> with Flink.
        /// </summary>
        /// <param name="name">name of the counter</param>
        /// <returns>the created counter</returns>
        ICounter Counter(string name);

        /// <summary>
        /// Registers a <see cref="ICounter"/>} with Flink.
        /// </summary>
        /// <typeparam name="TCounter">counter type</typeparam>
        /// <param name="name">name of the counter</param>
        /// <param name="counter">counter to register</param>
        /// <returns></returns>
        TCounter Counter<TCounter>(int name, TCounter counter) where TCounter : ICounter;

        /// <summary>
        /// Registers a <see cref="ICounter"/>} with Flink.
        /// </summary>
        /// <typeparam name="TCounter">counter type</typeparam>
        /// <param name="name">name of the counter</param>
        /// <param name="counter">counter to register</param>
        /// <returns></returns>
        TCounter Counter<TCounter>(string name, TCounter counter) where TCounter : ICounter;

        /// <summary>
        /// Registers a new <see cref="IGauge{TValue}"/> with Flink.
        /// </summary>
        /// <typeparam name="TGauge">return type of the gauge</typeparam>
        /// <param name="name">name of the gauge</param>
        /// <param name="gauge">gauge to register</param>
        /// <returns>the given gauge</returns>
        TGauge Gauge<TGauge>(int name, TGauge gauge) where TGauge : IGauge<TGauge>;

        /// <summary>
        /// Registers a new <see cref="IGauge{TValue}"/> with Flink.
        /// </summary>
        /// <typeparam name="TGauge">return type of the gauge</typeparam>
        /// <param name="name">name of the gauge</param>
        /// <param name="gauge">gauge to register</param>
        /// <returns>the given gauge</returns>
        TGauge Gauge<TGauge>(string name, TGauge gauge) where TGauge : IGauge<TGauge>;

        /// <summary>
        /// Registers a new <see cref="IHistogram"/> with Flink.
        /// </summary>
        /// <typeparam name="THistogram">return type of the histogram</typeparam>
        /// <param name="name">name of the histogram</param>
        /// <param name="histogram">histogram to register</param>
        /// <returns>the given histogram</returns>
        THistogram Histogram<THistogram>(int name, THistogram histogram) where THistogram : IHistogram;

        /// <summary>
        /// Registers a new <see cref="IHistogram"/> with Flink.
        /// </summary>
        /// <typeparam name="THistogram">return type of the histogram</typeparam>
        /// <param name="name">name of the histogram</param>
        /// <param name="histogram">histogram to register</param>
        /// <returns>the given histogram</returns>
        THistogram Histogram<THistogram>(string name, THistogram histogram) where THistogram : IHistogram;

        /// <summary>
        /// Registers a new <see cref="IMeter"/> with Flink.
        /// </summary>
        /// <typeparam name="TMeter">meter type</typeparam>
        /// <param name="name">name of the meter</param>
        /// <param name="meter">meter to register</param>
        /// <returns>the registered meter</returns>
        TMeter Meter<TMeter>(int name, TMeter meter) where TMeter : IMeter;

        /// <summary>
        /// Registers a new <see cref="IMeter"/> with Flink.
        /// </summary>
        /// <typeparam name="TMeter">meter type</typeparam>
        /// <param name="name">name of the meter</param>
        /// <param name="meter">meter to register</param>
        /// <returns>the registered meter</returns>
        TMeter Meter<TMeter>(string name, TMeter meter) where TMeter : IMeter;

        #endregion

        #region [ Groups ]

        /// <summary>
        /// Creates a new MetricGroup and adds it to this groups sub-groups.
        /// </summary>
        /// <param name="name">name of the group</param>
        /// <returns>the created group</returns>
        IMetricGroup AddGroup(int name);

        /// <summary>
        /// Creates a new MetricGroup and adds it to this groups sub-groups.
        /// </summary>
        /// <param name="name">name of the group</param>
        /// <returns>the created group</returns>
        IMetricGroup AddGroup(string name);

        /// <summary>
        /// Creates a new key-value MetricGroup pair.
        /// The key group is added to this groups sub-groups, while the value group is added to the key group's sub-groups. This method returns the value group.
        /// </summary>
        /// <param name="key">name of the first group</param>
        /// <param name="value">name of the second group</param>
        /// <returns>the second created group</returns>
        IMetricGroup AddGroup(string key, string value);

        #endregion

        #region [ Scope ]

        /// <summary>
        /// Gets the scope as an array of the scope components, for example <code>["host-7", "taskmanager-2", "window_word_count", "my-mapper"]</code>.
        /// </summary>
        string[] ScopeComponents { get; }

        /// <summary>
        /// Returns a map of all variables and their associated value, for example <code>{"<host>"="host-7", "<tm_id>"="taskmanager-2"}</code>.
        /// </summary>
        Dictionary<string, string> AllVariables { get; }

        /// <summary>
        /// Returns the fully qualified metric name, for example <code>"host-7.taskmanager-2.window_word_count.my-mapper.metricName"</code>
        /// </summary>
        /// <param name="metricName">metric name</param>
        /// <returns>fully qualified metric name</returns>
        string GetMetricIdentifier(string metricName);

        /// <summary>
        /// Returns the fully qualified metric name, for example <code>"host-7.taskmanager-2.window_word_count.my-mapper.metricName"</code>
        /// </summary>
        /// <param name="metricName">metric name</param>
        /// <param name="filter">character filter which is applied to the scope components if not null.</param>
        /// <returns>fully qualified metric name</returns>
        string GetMetricIdentifier(string metricName, ICharacterFilter filter);

        #endregion
    }
}
