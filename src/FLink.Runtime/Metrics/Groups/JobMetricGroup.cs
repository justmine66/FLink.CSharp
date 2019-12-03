using System.Collections.Generic;
using FLink.Core.Api.Common;
using FLink.Metrics.Core;

namespace FLink.Runtime.Metrics.Groups
{
    public class JobMetricGroup<TComponent> : ComponentMetricGroup<TComponent> where TComponent : ComponentMetricGroup<TComponent>
    {
        /// <summary>
        /// The ID of the job represented by this metrics group.
        /// </summary>
        public JobId JobId;

        /// <summary>
        /// The name of the job represented by this metrics group.
        /// </summary>
        public string JobName;

        public JobMetricGroup(
            IMetricRegistry registry,
            TComponent parent,
            JobId jobId,
            string jobName,
            string[] scope)
            : base(registry, scope, parent)
        {
        }

        protected override void PutVariables(Dictionary<string, string> tmpVariables)
        {
            throw new System.NotImplementedException();
        }

        protected override string GetGroupName(ICharacterFilter filter)
        {
            throw new System.NotImplementedException();
        }

        protected override IEnumerable<ComponentMetricGroup<TComponent>> SubComponents { get; }
    }
}
