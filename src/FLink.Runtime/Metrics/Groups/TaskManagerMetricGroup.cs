using System.Collections.Generic;
using FLink.Metrics.Core;

namespace FLink.Runtime.Metrics.Groups
{
    public class TaskManagerMetricGroup : ComponentMetricGroup<TaskManagerMetricGroup>
    {
        public string Hostname;

        public string TaskManagerId;

        public TaskManagerMetricGroup(
            IMetricRegistry registry,
            string hostname,
            string taskManagerId)
            : base(registry, null, null)
        {
            Hostname = hostname;
            TaskManagerId = taskManagerId;
        }

        protected override void PutVariables(Dictionary<string, string> tmpVariables)
        {
            throw new System.NotImplementedException();
        }

        protected override string GetGroupName(ICharacterFilter filter) => "taskmanager";

        protected override IEnumerable<ComponentMetricGroup<TaskManagerMetricGroup>> SubComponents { get; }
    }
}
