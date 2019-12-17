namespace FLink.Streaming.Runtime.StreamStatuses
{
    /// <summary>
    /// Interface for retrieving the current <see cref="StreamStatus"/>.
    /// </summary>
    public interface IStreamStatusProvider
    {
        /// <summary>
        /// Gets the current stream status.
        /// </summary>
        StreamStatus StreamStatus { get; }
    }
}
