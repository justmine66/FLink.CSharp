using FLink.Streaming.Api.Functions;
using FLink.Streaming.Api.Watermarks;
using FLink.Streaming.Runtime.StreamRecord;
using FLink.Streaming.Runtime.Tasks;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// A stream operator that extracts timestamps from stream elements and generates periodic watermarks.
    /// </summary>
    /// <typeparam name="T">The type of the input elements</typeparam>
    public class TimestampsAndPeriodicWatermarksOperator<T>: AbstractUdfStreamOperator<T, IAssignerWithPeriodicWatermarks<T>>, IOneInputStreamOperator<T, T>, IProcessingTimeCallback
    {
        public TimestampsAndPeriodicWatermarksOperator(IAssignerWithPeriodicWatermarks<T> userFunction) 
            : base(userFunction)
        {
        }
       
        public void ProcessElement(StreamRecord<T> element)
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

        public void OnProcessingTime(long timestamp)
        {
            throw new System.NotImplementedException();
        }
    }
}
