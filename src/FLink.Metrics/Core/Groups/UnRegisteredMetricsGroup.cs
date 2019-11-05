using System.Collections.Generic;

namespace FLink.Metrics.Core.Groups
{
    /// <summary>
    /// A special <see cref="IMetricGroup"/> that does not register any metrics at the metrics registry and any reporters.
    /// </summary>
    public class UnRegisteredMetricsGroup : IMetricGroup
    {
        public ICounter Counter(int name) => new SimpleCounter();

        public ICounter Counter(string name) => new SimpleCounter();

        public TCounter Counter<TCounter>(int name, TCounter counter) where TCounter : ICounter => default;

        public TCounter Counter<TCounter>(string name, TCounter counter) where TCounter : ICounter => default;

        public TGauge Gauge<TGauge>(int name, TGauge gauge) where TGauge : IGauge<TGauge> => default;

        public TGauge Gauge<TGauge>(string name, TGauge gauge) where TGauge : IGauge<TGauge> => default;

        public THistogram Histogram<THistogram>(int name, THistogram histogram) where THistogram : IHistogram => default;

        public THistogram Histogram<THistogram>(string name, THistogram histogram) where THistogram : IHistogram => default;

        public TMeter Meter<TMeter>(int name, TMeter meter) where TMeter : IMeter => default;

        public TMeter Meter<TMeter>(string name, TMeter meter) where TMeter : IMeter => default;

        public IMetricGroup AddGroup(int name) => AddGroup(name.ToString());

        public IMetricGroup AddGroup(string name) => new UnRegisteredMetricsGroup();

        public IMetricGroup AddGroup(string key, string value) => new UnRegisteredMetricsGroup();

        public string[] ScopeComponents => new string[0];
        public Dictionary<string, string> AllVariables => new Dictionary<string, string>();
        public string GetMetricIdentifier(string metricName) => metricName;

        public string GetMetricIdentifier(string metricName, ICharacterFilter filter) => metricName;
    }
}
