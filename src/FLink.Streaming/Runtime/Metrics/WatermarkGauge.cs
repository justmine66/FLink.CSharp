using FLink.Metrics.Core;

namespace FLink.Streaming.Runtime.Metrics
{
    /// <summary>
    /// A <see cref="IGauge{TValue}"/> for exposing the current input/output watermark.
    /// </summary>
    public class WatermarkGauge : IGauge<long>
    {
        public long Value { get; set; }
    }
}
