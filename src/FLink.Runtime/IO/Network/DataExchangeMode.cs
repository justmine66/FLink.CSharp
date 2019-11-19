namespace FLink.Runtime.IO.Network
{
    /// <summary>
    /// Defines how the data exchange between two specific operators happens.
    /// </summary>
    public enum DataExchangeMode
    {
        /// <summary>
        /// The data exchange is streamed, sender and receiver are online at the same time, and the receiver back-pressures the sender.
        /// </summary>
        Pipelined,
        /// <summary>
        /// The data exchange is decoupled. The sender first produces its entire result and finishes. After that, the receiver is started and may consume the data.
        /// </summary>
        Batch,
        /// <summary>
        /// The data exchange starts like in <see cref="Pipelined"/> and falls back to <see cref="Batch"/> for recovery runs.
        /// </summary>
        PipelineWithBatchFallback
    }
}
