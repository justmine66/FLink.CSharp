using System;
using FLink.Core.IO;
using FLink.Runtime.IO.Network.Buffers;

namespace FLink.Runtime.IO.Network.Api.Serialization
{
    public class SpanningRecordSerializer<T> : IRecordSerializer<T> where T : IIOReadableWritable
    {
        public void SerializeRecord(T record)
        {
            throw new NotImplementedException();
        }

        public SerializationResult CopyToBufferBuilder(BufferBuilder bufferBuilder)
        {
            throw new NotImplementedException();
        }

        public void Prune()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public bool HasSerializedData { get; }
    }
}
