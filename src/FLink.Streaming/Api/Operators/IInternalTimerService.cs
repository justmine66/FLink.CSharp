namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// Interface for working with time and timers.
    /// </summary>
    /// <typeparam name="TNamespace">Type of the namespace to which timers are scoped.</typeparam>
    public interface IInternalTimerService<in TNamespace>
    {
        /// <summary>
        /// Returns the current processing time.
        /// </summary>
        long CurrentProcessingTime { get; }

        /// <summary>
        /// Returns the current event-time watermark.
        /// </summary>
        long CurrentWatermark { get; }

        /// <summary>
        /// Registers a timer to be fired when processing time passes the given time. The namespace you pass here will be provided when the timer fires.
        /// </summary>
        /// <param name="namespace"></param>
        /// <param name="time"></param>
        void RegisterProcessingTimeTimer(TNamespace @namespace, long time);

        /// <summary>
        /// Deletes the timer for the given key and namespace.
        /// </summary>
        /// <param name="namespace"></param>
        /// <param name="time"></param>
        void DeleteProcessingTimeTimer(TNamespace @namespace, long time);

        /// <summary>
        /// Registers a timer to be fired when event time watermark passes the given time. The namespace you pass here will be provided when the timer fires.
        /// </summary>
        /// <param name="namespace"></param>
        /// <param name="time"></param>
        void RegisterEventTimeTimer(TNamespace @namespace, long time);

        /// <summary>
        /// Deletes the timer for the given key and namespace.
        /// </summary>
        /// <param name="namespace"></param>
        /// <param name="time"></param>
        void DeleteEventTimeTimer(TNamespace @namespace, long time);
    }
}
