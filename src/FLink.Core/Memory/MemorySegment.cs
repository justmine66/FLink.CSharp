using System;
using FLink.Core.Exceptions;

namespace FLink.Core.Memory
{
    /// <summary>
    /// This class represents a piece of memory managed by Flink.
    /// The segment may be backed by heap memory (byte array) or by off-heap memory.
    /// All methods that operate across two memory segments are implemented in this class, to transparently handle the mixing of memory segment types.
    /// We add this specialized class for various reasons:
    /// 1. It offers additional binary compare, swap, and copy methods.
    /// 2. It uses collapsed checks for range check and memory segment disposal.
    /// 3. It offers absolute positioning methods for bulk put/get methods, to guarantee thread safe use.
    /// 4. It offers explicit big-endian / little-endian access methods, rather than tracking internally a byte order.
    /// </summary>
    public abstract class MemorySegment
    {
        /// <summary>
        /// The beginning of the byte array contents, relative to the byte array object.
        /// </summary>
        protected static readonly long ByteArrayBaseOffset = 0;

        /// <summary>
        /// The heap byte array object relative to which we access the memory.
        /// Is non-null if the memory is on the heap, and is null, if the memory is off the heap. If we have this buffer, we must never void this reference, or the memory segment will point to undefined addresses outside the heap and may in out-of-order execution cases cause segmentation faults.
        /// </summary>
        protected byte[] HeapMemory { get; }

        /// <summary>
        /// The address to the data, relative to the heap memory byte array. If the heap memory byte array is null, this becomes an absolute memory address outside the heap.
        /// </summary>
        protected long Address { get; private set; }

        /// <summary>
        /// The address one byte after the last addressable byte, i.e. <tt>address + size</tt> while the segment is not disposed.
        /// </summary>
        protected long AddressLimit { get; }

        /// <summary>
        /// Gets the size of the memory segment, in bytes.
        /// </summary>
        protected int Size { get; }

        protected object Owner { get; }

        protected MemorySegment(byte[] buffer, object owner)
        {
            HeapMemory = buffer ?? throw new ArgumentNullException($"{nameof(buffer)}");
            Address = ByteArrayBaseOffset;
            Size = buffer.Length;
            AddressLimit = Address + Size;
            Owner = owner;
        }

        protected MemorySegment(long offHeapAddress, int size, object owner)
        {
            if (offHeapAddress <= 0)
            {
                throw new IllegalArgumentException("negative pointer or size");
            }
            if (offHeapAddress >= long.MaxValue - int.MaxValue)
            {
                // this is necessary to make sure the collapsed checks are safe against numeric overflows
                throw new IllegalArgumentException(
                    $"Segment initialized with too large address: {offHeapAddress} ; Max allowed address is {(long.MaxValue - int.MaxValue - 1)}");
            }

            HeapMemory = null;
            Address = offHeapAddress;
            AddressLimit = Address + size;
            Size = size;
            Owner = owner;
        }

        /// <summary>
        /// true, if the memory segment has been freed, false otherwise.
        /// </summary>
        public bool IsFreed => Address > AddressLimit;

        /// <summary>
        /// Frees this memory segment.
        /// After this operation has been called, no further operations are possible on the memory segment and will fail. The actual memory (heap or off-heap) will only be released after this memory segment object has become garbage collected.
        /// </summary>
        public void Free()
        {
            // this ensures we can place no more data and trigger
            // the checks for the freed segment
            Address = AddressLimit + 1;
        }

        /// <summary>
        /// true, if the memory segment is backed by off-heap memory, false if it is backed by heap memory.
        /// </summary>
        public bool IsOffHeap=> HeapMemory == null;
    }
}
