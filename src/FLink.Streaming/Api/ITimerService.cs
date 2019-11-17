using FLink.Streaming.Api.DataStreams;

namespace FLink.Streaming.Api
{
    /// <summary>
    /// Interface for working with time and timers.
    /// </summary>
    public interface ITimerService
    {
        /// <summary>
        /// Gets the current processing time.
        /// </summary>
        long CurrentProcessingTime { get; }

        /// <summary>
        /// Gets the current event-time watermark.
        /// </summary>
        long CurrentWatermark { get; }

        /// <summary>
        /// Registers a timer to be fired when processing time passes the given time.
        /// Timers can internally be scoped to keys and/or windows. When you set a timer in a keyed context, such as in an operation on <see cref="KeyedStream{TElement, TKey}"/> then that context will also be active when you receive the timer notification.
        /// </summary>
        /// <param name="time"></param>
        void RegisterProcessingTimeTimer(long time);

        /// <summary>
        /// Registers a timer to be fired when the event time watermark passes the given time.
        /// Timers can internally be scoped to keys and/or windows. When you set a timer in a keyed context, such as in an operation on <see cref="KeyedStream{TElement, TKey}"/> then that context will also be active when you receive the timer notification.
        /// </summary>
        /// <param name="time"></param>
        void RegisterEventTimeTimer(long time);

        /// <summary>
        /// Deletes the processing-time timer with the given trigger time. This method has only an effect if such a timer was previously registered and did not already expire.
        /// Timers can internally be scoped to keys and/or windows. When you delete a timer, It is removed from the current keyed context.
        /// </summary>
        /// <param name="time"></param>
        void DeleteProcessingTimeTimer(long time);

        /// <summary>
        /// Deletes the event-time timer with the given trigger time. This method has only an effect if such a timer was previously registered and did not already expire.
        /// Timers can internally be scoped to keys and/or windows. When you delete a timer,it is removed from the current keyed context.
        /// </summary>
        /// <param name="time"></param>
        void DeleteEventTimeTimer(long time);
    }
}
