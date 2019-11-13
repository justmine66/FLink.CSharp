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
    }
}
