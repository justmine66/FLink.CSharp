using System;
using FLink.Core.IO;
using FLink.Runtime.IO.Network.Buffers;

namespace FLink.Runtime.IO.Network.Api.Serialization
{
    public class SpillingAdaptiveSpanningRecordDeserializer<T> : IRecordDeserializer<T> where T : IIOReadableWritable
    {
        public SpillingAdaptiveSpanningRecordDeserializer(string[] tmpDirectories)
        {

        }

        public DeserializationResult GetNextRecord(T target)
        {
            throw new NotImplementedException();
        }

        public IBuffer Buffer { get; set; }
        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool HasUnfinishedData { get; }
    }
}
