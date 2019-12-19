using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FLink.Core.Api.Common;

namespace FLink.Core.Execution
{
    /// <summary>
    /// A client that is scoped to a specific job.
    /// </summary>
    public interface IJobClient
    {
        /// <summary>
        /// Gets the <see cref="JobId"/> that uniquely identifies the job this client is scoped to.
        /// </summary>
        JobId JobId { get; }

        /// <summary>
        /// Requests the {@link JobStatus} of the associated job.
        /// </summary>
        TaskCompletionSource<JobStatus> JobStatus { get; }

        /// <summary>
        /// Cancels the associated job.
        /// </summary>
        TaskCompletionSource<bool> Cancel();

        /// <summary>
        /// Stops the associated job on Flink cluster.
        /// Stopping works only for streaming programs. Be aware, that the job might continue to run for a while after sending the stop command, because after sources stopped to emit data all operators need to finish processing.
        /// </summary>
        /// <param name="advanceToEndOfEventTime">Flag indicating if the source should inject a max watermark in the pipeline</param>
        /// <param name="savepointDirectory">The savepoint should be written to</param>
        /// <returns>a <see cref="TaskCompletionSource{TResult}"/> containing the path where the savepoint is located</returns>
        TaskCompletionSource<string> StopWithSavepoint(bool advanceToEndOfEventTime, string savepointDirectory = null);

        /// <summary>
        /// Triggers a savepoint for the associated job. The savepoint will be written to the given savepoint directory.
        /// </summary>
        /// <param name="savepointDirectory"></param>
        /// <returns>a <see cref="TaskCompletionSource{TResult}"/> containing the path where the savepoint is located</returns>
        TaskCompletionSource<string> TriggerSavepoint(string savepointDirectory = null);

        /// <summary>
        /// Requests the accumulators of the associated job. Accumulators can be requested while it is running or after it has finished.The class loader is used to deserialize the incoming accumulator results.
        /// </summary>
        /// <param name="classType"></param>
        /// <returns></returns>
        TaskCompletionSource<IDictionary<string, object>> GetAccumulators(Type classType);

        /// <summary>
        /// Returns the result of the job execution of the submitted job.
        /// </summary>
        /// <param name="userClassType"></param>
        /// <returns></returns>
        TaskCompletionSource<JobExecutionResult> GetJobExecutionResult(Type userClassType);
    }
}
