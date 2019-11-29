using FLink.Streaming.Api.Watermarks;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// An entity keeping all the time-related services available to all operators extending the <see cref="AbstractStreamOperator{TOutput}"/>. 
    /// </summary>
    /// <typeparam name="K">The type of keys used for the timers and the registry.</typeparam>
    public class InternalTimeServiceManager<K>
    {
        public void AdvanceWatermark(Watermark watermark)
        {

        }
    }
}
