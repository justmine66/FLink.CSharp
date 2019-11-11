namespace FLink.Streaming.Api.Transformations
{
    /// <summary>
    /// The shuffle mode defines the data exchange mode between operators.
    /// </summary>
    public enum ShuffleMode
    {
        /// <summary>
        /// Producer and consumer are online at the same time.
        /// Produced data is received by consumer immediately.
        /// </summary>
        Pipelined,
        /// <summary>
        /// The producer first produces its entire result and finishes.
        /// After that, the consumer is started and may consume the data.
        /// </summary>
        Batch,
        /// <summary>
        /// The shuffle mode is undefined. It leaves it up to the framework to decide the shuffle mode. The framework will pick one of <see cref="Pipelined"/> or <see cref="Batch"/> in the end.
        /// </summary>
        Undefined
    }
}
