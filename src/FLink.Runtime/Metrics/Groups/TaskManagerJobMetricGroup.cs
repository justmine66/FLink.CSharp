using FLink.Core.Api.Common;

namespace FLink.Runtime.Metrics.Groups
{
    public class TaskManagerJobMetricGroup : JobMetricGroup<TaskManagerMetricGroup>
    {
        public TaskManagerJobMetricGroup(
            IMetricRegistry registry, 
            TaskManagerMetricGroup parent, 
            JobId jobId, string jobName) 
            : base(registry, parent, jobId, jobName, null)
        {
        }
    }
}
