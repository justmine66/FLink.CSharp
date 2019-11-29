using FLink.Metrics.Core;
using FLink.Runtime.JobGraphs;
using FLink.Streaming.Api.Functions.Sink;
using FLink.Streaming.Api.Watermarks;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// A <see cref="IStreamOperator{TOutput}"/> for executing SinkFunctions.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    public class StreamSink<TInput> : AbstractUdfStreamOperator<TInput, ISinkFunction<TInput>>, IOneInputStreamOperator<TInput, object>
    {
        public StreamSink(ISinkFunction<TInput> userFunction) : base(userFunction)
        {
        }

        public void ProcessElement(StreamRecord<TInput> element)
        {
            throw new System.NotImplementedException();
        }

        public void ProcessWatermark(Watermark mark)
        {
            throw new System.NotImplementedException();
        }

        public void ProcessLatencyMarker(LatencyMarker latencyMarker)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void SetKeyContextElement1<T>(StreamRecord<T> record)
        {
            throw new System.NotImplementedException();
        }

        public void SetKeyContextElement2<T>(StreamRecord<T> record)
        {
            throw new System.NotImplementedException();
        }

        public ChainingStrategy ChainingStrategy { get; set; }
        public IMetricGroup MetricGroup { get; }
        public OperatorId OperatorId { get; }
    }
}
