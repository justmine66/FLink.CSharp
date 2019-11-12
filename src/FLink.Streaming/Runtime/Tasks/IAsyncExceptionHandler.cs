using System;

namespace FLink.Streaming.Runtime.Tasks
{
    /// <summary>
    /// An interface marking a task as capable of handling exceptions thrown by different threads, other than the one executing the task itself.
    /// </summary>
    public interface IAsyncExceptionHandler
    {
        /// <summary>
        /// Handles an exception thrown by another thread (e.g. a TriggerTask),other than the one executing the main task.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void HandleAsyncException(string message, Exception exception);
    }
}
