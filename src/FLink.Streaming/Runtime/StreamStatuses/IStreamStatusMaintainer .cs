namespace FLink.Streaming.Runtime.StreamStatuses
{
    /// <summary>
    /// Interface that allows toggling the current <see cref="StreamStatus"/> as well as retrieving it.
    /// </summary>
    public interface IStreamStatusMaintainer : IStreamStatusProvider
    {
        /// <summary>
        /// Toggles the current stream status. This method should only have effect if the supplied stream status is different from the current status.
        /// </summary>
        /// <param name="streamStatus"></param>
        void ToggleStreamStatus(StreamStatus streamStatus);
    }
}
