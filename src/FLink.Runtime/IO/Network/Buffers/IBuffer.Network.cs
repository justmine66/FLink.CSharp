using FLink.Core.Memory;
using FLink.Core.Util;

namespace FLink.Runtime.IO.Network.Buffers
{
    /// <summary>
    /// Wrapper for pooled <see cref="MemorySegment"/> instances.
    /// </summary>
    public class NetworkBuffer : IBuffer
    {
        /// <summary>
        /// The current size of the buffer in the range from 0 (inclusive) to the size of the backing <see cref="MemorySegment"/> (inclusive).
        /// </summary>
        private readonly int _currentSize;
        /// <summary>
        /// The recycler for the backing <see cref="MemorySegment"/>.
        /// </summary>
        private readonly IBufferRecycler _recycler;

        /// <summary>
        /// Creates a new buffer instance backed by the given <tt>memorySegment</tt> with <tt>0</tt> for the<tt> readerIndex</tt> and<tt> size</tt> as <tt>writerIndex</tt>.
        /// </summary>
        /// <param name="memorySegment">backing memory segment (defines <see cref="MaxCapacity"/>)</param>
        /// <param name="recycler">will be called to recycle this buffer once the reference count is <tt>0</tt></param>
        /// <param name="isBuffer">whether this buffer represents a buffer (<tt>true</tt>) or an event (<tt>false</tt>)</param>
        /// <param name="size">current size of data in the buffer, i.e. the writer index to set</param>
        public NetworkBuffer(MemorySegment memorySegment, IBufferRecycler recycler, bool isBuffer = true, int size = 0)
        {
            MemorySegment = Preconditions.CheckNotNull(memorySegment);
            IsBuffer = isBuffer;
            _currentSize = memorySegment.Size;
            _recycler = recycler;
            Size = size;
        }


        public bool IsBuffer { get; }

        public void TagAsEvent()
        {
            throw new System.NotImplementedException();
        }

        public void RecycleBuffer()
        {
            throw new System.NotImplementedException();
        }

        public bool IsRecycled { get; }

        public IBuffer RetainBuffer()
        {
            throw new System.NotImplementedException();
        }

        public IBuffer ReadOnlySlice()
        {
            throw new System.NotImplementedException();
        }

        public IBuffer ReadOnlySlice(int index, int length)
        {
            throw new System.NotImplementedException();
        }

        public int MaxCapacity { get; }
        public int ReaderIndex { get; set; }
        public int SizeUnsafe { get; }
        public int Size { get; set; }
        public int ReadableBytes { get; set; }
        public MemorySegment MemorySegment { get; }
        public int MemorySegmentOffset { get; }
    }
}
