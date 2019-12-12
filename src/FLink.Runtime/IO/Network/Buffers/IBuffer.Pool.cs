namespace FLink.Runtime.IO.Network.Buffers
{
    /// <summary>
    /// A dynamically sized buffer pool.
    /// </summary>
    public interface IBufferPool : IBufferProvider, IBufferRecycler
    {
        /// <summary>
        /// Destroys this buffer pool.
        /// If not all buffers are available, they are recycled lazily as soon as they are recycled.
        /// </summary>
        void LazyDestroy();

        /// <summary>
        /// Checks whether this buffer pool has been destroyed.
        /// </summary>
        new bool IsDestroyed { get; }

        /// <summary>
        /// Returns the number of guaranteed (minimum number of) memory segments of this buffer pool.
        /// </summary>
        int NumberOfRequiredMemorySegments { get; }

        /// <summary>
        /// Returns the maximum number of memory segments this buffer pool should use or -1 if unlimited.
        /// </summary>
        int MaxNumberOfMemorySegments { get; }

        /// <summary>
        /// Gets and sets the current size of this buffer pool.
        /// The size of the buffer pool can change dynamically at runtime.
        /// The size needs to be greater or equal to the guaranteed number of memory segments.
        /// </summary>
        int NumBuffers { get; set; }

        /// <summary>
        /// Returns the number memory segments, which are currently held by this buffer pool.
        /// </summary>
        int NumberOfAvailableMemorySegments { get; }

        /// <summary>
        /// Returns the number of used buffers of this buffer pool.
        /// </summary>
        int BestEffortGetNumOfUsedBuffers { get; }
    }
}
