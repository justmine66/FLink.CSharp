using FLink.Core.Memory;

namespace FLink.Runtime.IO.Network.Buffer
{
    /// <summary>
    /// Interface for recycling <see cref="MemorySegment"/>s.
    /// </summary>
    public interface IBufferRecycler
    {
        /// <summary>
        /// Recycles the <see cref="MemorySegment"/> to its original <see cref="IBufferPool"/>.
        /// </summary>
        /// <param name="memorySegment">The memory segment to be recycled.</param>
        void Recycle(MemorySegment memorySegment);
    }
}
