﻿using System;

namespace FLink.Streaming.Api.Functions.Timestamps
{
    public abstract class AscendingTimestampExtractor<T> : IAssignerWithPeriodicWatermarks<T>
    {
        public long ExtractTimestamp(T element, long previousElementTimestamp)
        {
            throw new NotImplementedException();
        }

        public Watermark.Watermark CurrentWatermark { get; }
    }
}
