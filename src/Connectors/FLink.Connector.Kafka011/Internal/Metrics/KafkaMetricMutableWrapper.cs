using FLink.Metrics.Core;

namespace FLink.Connector.Kafka011.Internal.Metrics
{
    public class KafkaMetricMutableWrapper : IGauge<double>
    {
        public double Value { get;  }
    }
}
