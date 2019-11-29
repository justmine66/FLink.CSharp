using System.Threading.Tasks;

namespace FLink.Streaming.Runtime.Tasks
{
    /// <summary>
    /// Defines the current processing time and handles all related actions, such as register timers for tasks to be executed in the future.
    /// </summary>
    public interface IProcessingTimeService
    {
        /// <summary>
        /// Gets the current processing time.
        /// </summary>
        long CurrentProcessingTime { get; }

        /// <summary>
        /// Registers a task to be executed when (processing) time is timestamp.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="timestamp">Time when the task is to be executed (in processing time)</param>
        /// <param name="target">The task to be executed</param>
        /// <returns>The future that represents the scheduled task. This always returns some future, even if the timer was shut down.</returns>
        TaskCompletionSource<TResult> RegisterTimer<TResult>(long timestamp, IProcessingTimeCallback target);

        /// <summary>
        /// Registers a task to be executed repeatedly at a fixed rate.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="callback">to be executed after the initial delay and then after each period</param>
        /// <param name="initialDelay">initial delay to start executing callback</param>
        /// <param name="period">after the initial delay after which the callback is executed</param>
        /// <returns>Scheduled future representing the task to be executed repeatedly</returns>
        TaskCompletionSource<TResult> ScheduleAtFixedRate<TResult>(IProcessingTimeCallback callback, long initialDelay, long period);
    }
}
