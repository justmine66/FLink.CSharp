using FLink.Core.Api.Common;
using FLink.Optimizer.Plan;
using FLink.Runtime.JobGraphs;

namespace FLink.Streaming.Api.Graph
{
    /// <summary>
    /// Class representing the streaming topology. It contains all the information necessary to build the job graph for the execution.
    /// </summary>
    public class StreamGraph : StreamingPlan
    {
        public override JobGraph GetJobGraph(JobId jobId)
        {
            throw new System.NotImplementedException();
        }

        public override string GetStreamingPlanAsJson()
        {
            throw new System.NotImplementedException();
        }
    }
}
