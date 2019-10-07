namespace FLink.Streaming.Runtime.Tasks
{
    /// <summary>
    /// Interface for processing-time callbacks that can be registered at a <see cref="ProcessingTimeService"/>.
    /// </summary>
    public interface IProcessingTimeCallback
    {
        /// <summary>
        /// This method is invoked with the timestamp for which the trigger was scheduled.
        /// </summary>
        /// <param name="timestamp">The timestamp for which the trigger event was scheduled.</param>
        void OnProcessingTime(long timestamp);
    }
}
