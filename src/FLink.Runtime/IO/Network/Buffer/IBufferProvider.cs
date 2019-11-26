namespace FLink.Runtime.IO.Network.Buffer
{
    /// <summary>
    /// A buffer provider to request buffers from in a synchronous or asynchronous fashion.
    /// The data producing side (result partition writers) request buffers in a synchronous fashion, whereas the input side requests asynchronously.
    /// </summary>
    public interface IBufferProvider
    {
        /// <summary>
        /// Returns a <see cref="IBuffer"/> instance from the buffer provider, if one is available.
        /// Returns <code>null</code> if no buffer is available or the buffer provider has been destroyed.
        /// </summary>
        /// <returns></returns>
        IBuffer RequestBuffer();

        /// <summary>
        /// Returns a <see cref="IBuffer"/> instance from the buffer provider.
        /// If there is no buffer available, the call will block until one becomes available again or the buffer provider has been destroyed.
        /// </summary>
        /// <returns></returns>
        IBuffer RequestBufferBlocking();

        /// <summary>
        /// Gets whether the buffer provider has been destroyed.
        /// </summary>
        bool IsDestroyed { get; }

        /// <summary>
        /// Gets the size of the underlying memory segments. This is the maximum size a <see cref="IBuffer"/> instance can have.
        /// </summary>
        int MemorySegmentSize { get; }
    }
}
