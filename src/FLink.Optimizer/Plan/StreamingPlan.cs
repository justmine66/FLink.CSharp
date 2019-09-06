using System.IO;
using FLink.Core.Api.Common;
using FLink.Runtime.JobGraphs;

namespace FLink.Optimizer.Plan
{
    /// <summary>
    /// Abstract class representing FLink Streaming plans
    /// </summary>
    public abstract class StreamingPlan
    {
        public JobGraph GetJobGraph()
        {
            return GetJobGraph(null);
        }

        public abstract JobGraph GetJobGraph(JobId jobId);

        public abstract string GetStreamingPlanAsJson();

        public abstract void DumpStreamingPlanAsJson(FileInfo file);
    }
}
