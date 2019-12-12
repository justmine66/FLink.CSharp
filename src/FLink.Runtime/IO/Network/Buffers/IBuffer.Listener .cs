using System;

namespace FLink.Runtime.IO.Network.Buffers
{
    /// <summary>
    /// Interface of the availability of buffers.
    /// Listeners can opt for a one-time only notification or to be notified repeatedly.
    /// </summary>
    public interface IBufferListener
    {
        /// <summary>
        /// Notification callback if a buffer is recycled and becomes available in buffer pool.
        /// Note: responsibility on recycling the given buffer is transferred to this implementation, including any errors that lead to exceptions being thrown!
        /// BEWARE: since this may be called from outside the thread that relies on the listener's logic, any exception that occurs with this handler should be forwarded to the responsible thread for handling and otherwise ignored in the processing of this method. The buffer pool forwards any <see cref="Exception"/> from here upwards to a potentially unrelated call stack!
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        NotificationResult NotifyBufferAvailable(IBuffer buffer);

        /// <summary>
        /// Notification callback if the buffer provider is destroyed.
        /// </summary>
        void NotifyBufferDestroyed();
    }

    /// <summary>
    /// Status of the notification result from the buffer listener.
    /// </summary>
    public class NotificationResult
    {
        public static readonly NotificationResult BufferNotUsed = new NotificationResult(false, false);
        public static readonly NotificationResult BufferUsedNoNeedMore = new NotificationResult(true, false);
        public static readonly NotificationResult BufferUsedNeedMore = new NotificationResult(true, true);

        public NotificationResult(bool isBufferUsed, bool needsMoreBuffers)
        {
            IsBufferUsed = isBufferUsed;
            NeedsMoreBuffers = needsMoreBuffers;
        }

        /// <summary>
        /// Whether the notified buffer is accepted to use by the listener.
        /// true if the notified buffer is accepted.
        /// </summary>
        public bool IsBufferUsed { get; }

        /// <summary>
        /// Whether the listener still needs more buffers to be notified.
        /// true if the listener is still waiting for more buffers.
        /// </summary>
        public bool NeedsMoreBuffers { get; }
    }
}
