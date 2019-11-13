using FLink.Metrics.Core;
using FLink.Runtime.JobGraphs;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Util
{
    /// <summary>
    /// The <see cref="LatencyStats"/> objects are used to track and report on the behavior of latencies across measurements.
    /// </summary>
    public class LatencyStats
    {
        public void ReportLatency(LatencyMarker marker) { }

        public abstract class Granularity
        {
            public abstract string CreateUniqueHistogramName(LatencyMarker marker, OperatorId operatorId, int operatorSubTaskIndex);
            public abstract IMetricGroup CreateSourceMetricGroups(IMetricGroup @base, LatencyMarker marker, OperatorId operatorId, int operatorSubTaskIndex);
        }
    }
}
