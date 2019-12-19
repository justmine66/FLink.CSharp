using System;
using FLink.Core.Api.Common;

namespace FLink.Core.Execution
{
    /// <summary>
    /// A listener that is notified on specific job status changed.
    /// It is highly recommended NOT to perform any blocking operation inside the callbacks.
    /// If you block the thread the invoker of environment execute methods is possibly blocked.
    /// </summary>
    public interface IJobListener
    {
        /// <summary>
        /// Callback on job submission. This is called when StreamExecutionEnvironment.Execute() is called.
        /// Exactly one of the passed parameters is null, respectively for failure or success.
        /// </summary>
        /// <param name="jobClient">a <see cref="jobClient"/> for the submitted Flink job</param>
        /// <param name="exception">the cause if submission failed</param>
        void OnJobSubmitted(IJobClient jobClient = null, Exception exception = null);

        /// <summary>
        /// Callback on job execution finished, successfully or unsuccessfully. It is only called back when you call execute() instead of executeAsync() methods of execution environments.
        /// Exactly one of the passed parameters is null, respectively for failure or success.
        /// </summary>
        /// <param name="jobExecutionResult"></param>
        /// <param name="throwable"></param>
        void OnJobExecuted(JobExecutionResult jobExecutionResult = null, Exception throwable = null);
    }
}
