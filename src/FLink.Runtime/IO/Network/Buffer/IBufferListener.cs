namespace FLink.Runtime.IO.Network.Buffer
{
    /// <summary>
    /// Interface of the availability of buffers. Listeners can opt for a one-time only notification or to be notified repeatedly.
    /// </summary>
    public interface IBufferListener
    {
        /// <summary>
        /// Notification callback if a buffer is recycled and becomes available in buffer pool.
        /// </summary>
        /// <param name="buffer">that becomes available in buffer pool.</param>
        /// <returns>NotificationResult if the listener wants to be notified next time.</returns>
        NotificationResult NotifyBufferAvailable(IBuffer buffer);

        void NotifyBufferDestroyed();
    }

    /// <summary>
    /// Status of the notification result from the buffer listener.
    /// </summary>
    public class NotificationResult
    {
        public static NotificationResult BufferNotUsed = new NotificationResult(false, false);
        public static NotificationResult BufferUsedNoNeedMore = new NotificationResult(true, false);
        public static NotificationResult BufferUsedNeedMore = new NotificationResult(true, true);

        public NotificationResult(bool isBufferUsed, bool needsMoreBuffers)
        {
            IsBufferUsed = isBufferUsed;
            NeedsMoreBuffers = needsMoreBuffers;
        }

        /// <summary>
        /// True if the notified buffer is accepted.
        /// Whether the notified buffer is accepted to use by the listener.
        /// </summary>
        public bool IsBufferUsed { get; }

        /// <summary>
        /// True if the listener is still waiting for more buffers.
        /// Whether the listener still needs more buffers to be notified.
        /// </summary>
        public bool NeedsMoreBuffers { get; }
    }
}
