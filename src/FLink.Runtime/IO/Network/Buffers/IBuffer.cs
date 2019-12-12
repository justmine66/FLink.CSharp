using System;
using FLink.Core.Memory;

namespace FLink.Runtime.IO.Network.Buffers
{
    /// <summary>
    /// Wrapper for pooled <see cref="MemorySegment"/> instances with reference counting.
    /// This is similar to Netty's <tt>ByteBuf</tt> with some extensions and restricted to the methods our use cases outside Netty handling use. In particular, we use two different indexes for read and write operations, i.e. the <tt>reader</tt> and <tt>writer</tt> index (size of written data), which specify three regions inside the memory segment:
    ///     +-------------------+----------------+----------------+
    ///     | discardable bytes | readable bytes | writable bytes |
    ///     +-------------------+----------------+----------------+
    ///     |                   |                |                |
    ///     0      <=      readerIndex  <=  writerIndex   <=  max capacity
    /// </summary>
    public interface IBuffer
    {
        /// <summary>
        /// Gets whether this buffer represents a buffer or an event.
        /// true if this is a real buffer, false if this is an event
        /// </summary>
        bool IsBuffer { get; }

        /// <summary>
        /// Tags this buffer to represent an event.
        /// </summary>
        void TagAsEvent();

        /// <summary>
        /// Releases this buffer once, i.e. reduces the reference count and recycles the buffer if the reference count reaches 0.
        /// </summary>
        void RecycleBuffer();

        /// <summary>
        /// Gets whether this buffer has been recycled or not.
        /// true if already recycled, false otherwise
        /// </summary>
        bool IsRecycled { get; }

        /// <summary>
        /// Retains this buffer for further use, increasing the reference counter by 1.
        /// </summary>
        /// <returns>this instance (for chained calls)</returns>
        IBuffer RetainBuffer();

        /// <summary>
        /// Returns a read-only slice of this buffer's readable bytes, i.e. between .
        /// Reader and writer indices as well as markers are not shared. Reference counters are shared but the slice is not <see cref="RetainBuffer()"/> automatically
        /// </summary>
        /// <returns>a read-only sliced buffer</returns>
        IBuffer ReadOnlySlice();

        /// <summary>
        /// Returns a read-only slice of this buffer.
        /// Reader and writer indices as well as markers are not shared. Reference counters are shared but the slice is not <see cref="RetainBuffer()"/> automatically
        /// </summary>
        /// <param name="index">the index to start from</param>
        /// <param name="length">the length of the slice</param>
        /// <returns>a read-only sliced buffer</returns>
        IBuffer ReadOnlySlice(int index, int length);

        /// <summary>
        /// Gets the maximum size of the buffer, i.e. the capacity of the underlying <see cref="MemorySegment"/>.
        /// </summary>
        int MaxCapacity { get; }

        /// <summary>
        /// Gets and sets the reader index of this buffer.
        /// This is where readable (unconsumed) bytes start in the backing memory segment.
        /// reader index (from 0 (inclusive) to the size of the backing <see cref="MemorySegment"/> (inclusive))
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">When set the reader index of this buffer, if the read index is less than 0 or greater than <see cref="Size"/></exception>
        int ReaderIndex { get; set; }

        /// <summary>
        /// Gets the size of the written data, i.e. the writer index, of this buffer in an non-synchronized fashion.
        /// </summary>
        int SizeUnsafe { get; }

        /// <summary>
        /// Gets and sets the size of the written data, i.e. the writer index of this buffer.
        /// This is where writable bytes start in the backing memory segment. writer index (from 0 (inclusive) to the size of the backing <see cref="MemorySegment"/> (inclusive)).
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">When set the writer index of this buffer, if the writer index is less than <see cref="ReaderIndex"/> or greater than <see cref="MaxCapacity"/></exception>
        int Size { get; set; }

        /// <summary>
        /// Returns the number of readable bytes (same as <see cref="Size"/> - <see cref="ReaderIndex"/>).
        /// </summary>
        int ReadableBytes { get; set; }

        /// <summary>
        /// Returns the underlying memory segment backing this buffer. This method is dangerous since it ignores read only protections and omits slices.
        /// This method will be removed in the future. For writing use <see cref="BufferBuilder"/>.
        /// </summary>
        [Obsolete]
        MemorySegment MemorySegment { get; }

        /// <summary>
        /// the offset where this (potential slice) <see cref="IBuffer"/>'s data start in the underlying memory segment.
        /// This method will be removed in the future. For writing use <see cref="BufferBuilder"/>.
        /// </summary>
        [Obsolete]
        int MemorySegmentOffset { get; }
    }
}
