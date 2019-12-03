using System.Collections.Generic;
using FLink.Metrics.Core;

namespace FLink.Runtime.Metrics.Groups
{
    /// <summary>
    /// Abstract <see cref="IMetricGroup"/> for system components (e.g., TaskManager, Job, Task, Operator).
    /// </summary>
    /// <typeparam name="TParent"> The type of the parent MetricGroup.</typeparam>
    public abstract class ComponentMetricGroup<TParent> : AbstractMetricGroup<TParent> where TParent : AbstractMetricGroup<TParent>
    {
        /// <summary>
        /// Creates a new ComponentMetricGroup.
        /// </summary>
        /// <param name="registry">registry to register new metrics with</param>
        /// <param name="scope">the scope of the group</param>
        /// <param name="parent"></param>
        protected ComponentMetricGroup(IMetricRegistry registry, string[] scope, AbstractMetricGroup<TParent> parent)
            : base(registry, scope, parent)
        {
        }

        /// <summary>
        /// Gets all component metric groups that are contained in this component metric group.
        /// </summary>
        protected abstract IEnumerable<ComponentMetricGroup<TParent>> SubComponents { get; }
    }
}
