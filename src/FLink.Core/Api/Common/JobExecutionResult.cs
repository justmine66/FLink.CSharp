using System.Collections.Generic;
using FLink.Core.Util;

namespace FLink.Core.Api.Common
{
    /// <summary>
    /// The result of a job execution.
    /// Gives access to the execution time of the job, and to all accumulators created by this job.
    /// </summary>
    public class JobExecutionResult : JobSubmissionResult
    {
        private readonly long _netRuntime;
        private readonly Dictionary<string, OptionalFailure<object>> _accumulators;

        public JobExecutionResult(JobId jobId, long netRuntime,
            Dictionary<string, OptionalFailure<object>> accumulators)
            : base(jobId)
        {
            _netRuntime = netRuntime;
            _accumulators = accumulators ?? new Dictionary<string, OptionalFailure<object>>();
        }

        public new bool IsJobExecutionResult => true;

        public override JobExecutionResult GetJobExecutionResult()
        {
            return this;
        }

        /// <summary>
        /// Gets the net execution time of the job, i.e., the execution time in the parallel system, without the pre-flight steps like the optimizer.
        /// </summary>
        /// <returns></returns>
        public long GetNetRuntime()
        {
            return _netRuntime;
        }
    }
}
