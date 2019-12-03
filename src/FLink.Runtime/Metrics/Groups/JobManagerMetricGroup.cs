using System;
using System.Collections.Generic;
using FLink.Metrics.Core;

namespace FLink.Runtime.Metrics.Groups
{
    public class JobManagerMetricGroup : ComponentMetricGroup<JobManagerMetricGroup>
    {
        public string Hostname;

        public JobManagerMetricGroup(IMetricRegistry registry, string hostname)
            : base(registry, null, null)
        {
            Hostname = hostname;
        }

        protected override void PutVariables(Dictionary<string, string> tmpVariables)
        {
            throw new NotImplementedException();
        }

        protected override string GetGroupName(ICharacterFilter filter)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<ComponentMetricGroup<JobManagerMetricGroup>> SubComponents { get; }
    }
}
