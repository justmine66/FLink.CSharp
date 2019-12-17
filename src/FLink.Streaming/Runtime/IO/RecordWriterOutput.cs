using FLink.Core.Util;
using FLink.Metrics.Core;
using FLink.Runtime.IO.Network.Api.Writer;
using FLink.Streaming.Api.Operators;
using FLink.Streaming.Api.Watermarks;
using FLink.Streaming.Runtime.StreamRecords;
using FLink.Streaming.Runtime.Tasks;

namespace FLink.Streaming.Runtime.IO
{
    /// <summary>
    /// Implementation of <see cref="IOutput{TElement}"/> that sends data using a <see cref="RecordWriter{TRecord}"/>.
    /// </summary>
    /// <typeparam name="TOutput"></typeparam>
    public class RecordWriterOutput<TOutput> : IWatermarkGaugeExposingOutput<TOutput>
    {
        public void Collect(TOutput element)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            throw new System.NotImplementedException();
        }

        public void EmitWatermark(Watermark mark)
        {
            throw new System.NotImplementedException();
        }

        public void Collect<T>(OutputTag<T> outputTag, StreamRecord<T> record)
        {
            throw new System.NotImplementedException();
        }

        public void EmitLatencyMarker(LatencyMarker latencyMarker)
        {
            throw new System.NotImplementedException();
        }

        public IGauge<long> WatermarkGauge { get; }
    }
}
