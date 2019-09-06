using System;
using FLink.Streaming.Api.Watermarks;

namespace FLink.Streaming.Api.Functions.Timestamps
{
    public abstract class AscendingTimestampExtractor<T> : IAssignerWithPeriodicWatermarks<T>
    {
        public long ExtractTimestamp(T element, long previousElementTimestamp)
        {
            throw new NotImplementedException();
        }

        public Watermark CurrentWatermark { get; }
    }
}
