using FLink.Core.IO;
using FLink.Runtime.IO.Network.Buffer;

namespace FLink.Runtime.IO.Network.Api.Serialization
{
    /// <summary>
    /// Interface for turning records into sequences of memory segments.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRecordSerializer<in T> where T : IIOReadableWritable
    {
        /// <summary>
        /// Starts serializing the given record to an intermediate data buffer.
        /// </summary>
        /// <param name="record">To serialize</param>
        void SerializeRecord(T record);

        /// <summary>
        /// Copies the intermediate data serialization buffer to the given target buffer.
        /// </summary>
        /// <param name="bufferBuilder">the new target buffer to use</param>
        /// <returns>how much information was written to the target buffer and whether this buffer is full</returns>
        SerializationResult CopyToBufferBuilder(BufferBuilder bufferBuilder);

        /// <summary>
        /// Clears the buffer and checks to decrease the size of intermediate data serialization buffer after finishing the whole serialization process including <see cref="SerializeRecord"/> and <see cref="CopyToBufferBuilder"/>.
        /// </summary>
        void Prune();

        /// <summary>
        /// Supports copying an intermediate data serialization buffer to multiple target buffers by resetting its initial position before each copying.
        /// </summary>
        void Reset();

        /// <summary>
        /// true if has some serialized data pending copying to the result <see cref="BufferBuilder"/>.
        /// </summary>
        bool HasSerializedData { get; }
    }

    /// <summary>
    /// Status of the serialization result.
    /// </summary>
    public class SerializationResult
    {
        public static SerializationResult PartialRecordMemorySegmentFull = new SerializationResult(false, true);
        public static SerializationResult FullRecordMemorySegmentFull = new SerializationResult(true, true);
        public static SerializationResult FullRecord = new SerializationResult(true, false);

        public SerializationResult(bool isBufferUsed, bool needsMoreBuffers)
        {
            IsFullRecord = isBufferUsed;
            IsFullBuffer = needsMoreBuffers;
        }

        /// <summary>
        /// True if the complete record was written
        /// Whether the full record was serialized and completely written to.
        /// </summary>
        public bool IsFullRecord { get; }

        /// <summary>
        /// true if the target buffer is full
        /// Whether the target buffer is full after the serialization process.
        /// </summary>
        public bool IsFullBuffer { get; }
    }
}
