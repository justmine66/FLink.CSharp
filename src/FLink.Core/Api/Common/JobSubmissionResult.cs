using System;

namespace FLink.Core.Api.Common
{
    /// <summary>
    /// The result of submitting a job to a JobManager.
    /// </summary>
    public class JobSubmissionResult
    {
        private readonly JobId _jobId;

        public JobSubmissionResult(JobId jobId)
        {
            _jobId = jobId;
        }

        /// <summary>
        /// Returns the JobID assigned to the job by the FLink runtime.
        /// </summary>
        /// <returns>jobID, or null if the job has been executed on a runtime without JobIDs or if the execution failed.</returns>
        public JobId GetJobId()
        {
            return _jobId;
        }

        /// <summary>
        /// Checks if this JobSubmissionResult is also a JobExecutionResult.
        /// </summary>
        public bool IsJobExecutionResult = false;

        /// <summary>
        /// Returns the JobExecutionResult if available.
        /// </summary>
        /// <returns>The JobExecutionResult</returns>
        /// <exception cref="InvalidCastException">if this is not a JobExecutionResult.</exception>
        public virtual JobExecutionResult GetJobExecutionResult()
        {
            throw new InvalidCastException("This JobSubmissionResult is not a JobExecutionResult.");
        }
    }
}
