using FLink.Core.Api.Common;
using FLink.Runtime.JobGraphs;

namespace FLink.Optimizer.Plan
{
    /// <summary>
    /// Abstract class representing FLink Streaming plans
    /// </summary>
    public abstract class StreamingPlan
    {
        /// <summary>
        /// Gets the assembled <see cref="JobGraph"/> with a random <see cref="JobId"/>.
        /// </summary>
        /// <returns></returns>
        public JobGraph GetJobGraph()
        {
            return GetJobGraph(null);
        }

        /// <summary>
        /// Gets the assembled <see cref="JobGraph"/> with a specified <see cref="JobId"/>.
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public abstract JobGraph GetJobGraph(JobId jobId);

        public abstract string GetStreamingPlanAsJson();
    }
}
