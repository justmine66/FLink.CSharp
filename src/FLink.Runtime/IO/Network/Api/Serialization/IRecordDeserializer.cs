using FLink.Core.IO;
using FLink.Runtime.IO.Network.Buffer;

namespace FLink.Runtime.IO.Network.Api.Serialization
{
    /// <summary>
    /// Interface for turning sequences of memory segments into records.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRecordDeserializer<in T> where T : IIOReadableWritable
    {
        DeserializationResult GetNextRecord(T target);

        IBuffer Buffer { get; set; }

        void Clear();

        bool HasUnfinishedData { get; }
    }

    /// <summary>
    /// Status of the deserialization result.
    /// </summary>
    public class DeserializationResult
    {
        public static DeserializationResult PartialRecord = new DeserializationResult(false, true);
        public static DeserializationResult IntermediateRecordFromBuffer = new DeserializationResult(true, false);
        public static DeserializationResult LastRecordFromBuffer = new DeserializationResult(true, true);

        public DeserializationResult(bool isBufferUsed, bool needsMoreBuffers)
        {
            IsFullRecord = isBufferUsed;
            IsBufferConsumed = needsMoreBuffers;
        }

        public bool IsFullRecord { get; }

        public bool IsBufferConsumed { get; }
    }
}
