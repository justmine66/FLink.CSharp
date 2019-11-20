using System.Collections.Generic;
using System.Linq;
using FLink.Core.Util;
using FLink.Metrics.Core;
using FLink.Runtime.Metrics.Scope;

namespace FLink.Runtime.Metrics.Groups
{
    /// <summary>
    /// Abstract <see cref="IMetricGroup"/> that contains key functionality for adding metrics and groups.
    /// This class uses locks for adding and removing metrics objects. This is done to prevent resource leaks in the presence of concurrently closing a group and adding metrics and subgroups.
    /// Since closing groups recursively closes the subgroups, the lock acquisition order must be strictly from parent group to subgroup. If at any point, a subgroup holds its group lock and calls a parent method that also acquires the lock, it will create a deadlock condition.
    /// An AbstractMetricGroup can be {@link #close() closed}. the group de-register all metrics from any metrics reporter and any internal maps. Note that even closed metrics groups return Counters, Gauges, etc to the code, to prevent exceptions in the monitored code. These metrics simply do not get reported any more, when created on a closed group.
    /// </summary>
    /// <typeparam name="TParent">The type of the parent MetricGroup</typeparam>
    public abstract class AbstractMetricGroup<TParent> : IMetricGroup
    {
        /// <summary>
        /// The parent group containing this group.
        /// </summary>
        protected AbstractMetricGroup<TParent> Parent;

        /// <summary>
        /// The map containing all variables and their associated values, lazily computed.
        /// </summary>
        protected volatile Dictionary<string, string> Variables;

        /// <summary>
        /// The registry that this metrics group belongs to.
        /// </summary>
        protected IMetricRegistry Registry;

        // All metrics that are directly contained in this group.
        private readonly Dictionary<string, IMetric> _metrics = new Dictionary<string, IMetric>();

        // All metric subgroups of this group.
        private readonly Dictionary<string, AbstractMetricGroup<IMetricGroup>> _groups = new Dictionary<string, AbstractMetricGroup<IMetricGroup>>();

        // Array containing the metrics scope represented by this group for each reporter, as a concatenated string, lazily computed.
        // For example: "host-7.taskmanager-2.window_word_count.my-mapper" 
        private readonly string[] _scopeStrings;

        // The logical metrics scope represented by this group, as a concatenated string, lazily computed.
        // For example: "taskmanager.job.task" */
        private string _logicalScopeString;

        // Flag indicating whether this group has been closed.
        private volatile bool _closed;

        /// <summary>
        /// The metrics scope represented by this group.
        /// For example ["host-7", "taskmanager-2", "window_word_count", "my-mapper" ]. 
        /// </summary>
        public string[] ScopeComponents { get; }

        public Dictionary<string, string> AllVariables
        {
            get
            {
                if (Variables == null)// avoid synchronization for common case
                {
                    lock (this)
                    {
                        if (Variables == null)
                        {
                            var tmpVariables = new Dictionary<string, string>();
                            PutVariables(tmpVariables);
                            if (Parent != null)
                            {
                                // not true for Job-/TaskManagerMetricGroup
                                tmpVariables.Union(Parent.AllVariables);
                            }

                            Variables = tmpVariables;
                        }
                    }
                }

                return Variables;
            }
        }

        /// <summary>
        /// Enters all variables specific to this <see cref="AbstractMetricGroup{TParent}"/> and their associated values into the map.
        /// </summary>
        /// <param name="tmpVariables">map to enter variables and their values into</param>
        protected abstract void PutVariables(Dictionary<string, string> tmpVariables);

        protected AbstractMetricGroup(IMetricRegistry registry, string[] scope, AbstractMetricGroup<TParent> parent)
        {
            Registry = Preconditions.CheckNotNull(registry);
            ScopeComponents = Preconditions.CheckNotNull(scope);
            Parent = parent;
            _scopeStrings = new string[registry.NumberReporters];
        }

        /// <summary>
        /// Returns the logical scope of this group, for example "taskmanager.job.task"
        /// </summary>
        /// <param name="filter">character filter which is applied to the scope components</param>
        /// <returns>logical scope</returns>
        public string GetLogicalScope(ICharacterFilter filter) => GetLogicalScope(filter, Registry.Delimiter);

        /// <summary>
        /// Returns the logical scope of this group, for example "taskmanager.job.task"
        /// </summary>
        /// <param name="filter">character filter which is applied to the scope components</param>
        /// <param name="delimiter"></param>
        /// <returns>logical scope</returns>
        public string GetLogicalScope(ICharacterFilter filter, char delimiter)
        {
            if (_logicalScopeString == null)
            {
                if (Parent == null)
                {
                    _logicalScopeString = GetGroupName(filter);
                }
                else
                {
                    _logicalScopeString = Parent.GetLogicalScope(filter, delimiter) + delimiter + GetGroupName(filter);
                }
            }

            return _logicalScopeString;
        }

        protected abstract string GetGroupName(ICharacterFilter filter);

        public IMetricGroup AddGroup(int name)
        {
            throw new System.NotImplementedException();
        }

        public IMetricGroup AddGroup(string name)
        {
            throw new System.NotImplementedException();
        }

        public IMetricGroup AddGroup(string key, string value)
        {
            throw new System.NotImplementedException();
        }

        public ICounter Counter(int name)
        {
            throw new System.NotImplementedException();
        }

        public ICounter Counter(string name)
        {
            throw new System.NotImplementedException();
        }

        public TCounter Counter<TCounter>(int name, TCounter counter) where TCounter : ICounter
        {
            throw new System.NotImplementedException();
        }

        public TCounter Counter<TCounter>(string name, TCounter counter) where TCounter : ICounter
        {
            throw new System.NotImplementedException();
        }

        public TGauge Gauge<TGauge>(int name, TGauge gauge) where TGauge : IGauge<TGauge>
        {
            throw new System.NotImplementedException();
        }

        public TGauge Gauge<TGauge>(string name, TGauge gauge) where TGauge : IGauge<TGauge>
        {
            throw new System.NotImplementedException();
        }

        public string GetMetricIdentifier(string metricName) => GetMetricIdentifier(metricName, null);

        public string GetMetricIdentifier(string metricName, ICharacterFilter filter) => GetMetricIdentifier(metricName, filter, -1);

        /// <summary>
        /// Returns the fully qualified metric name using the configured delimiter for the reporter with the given index, for example "host-7.taskmanager-2.window_word_count.my-mapper.metricName".
        /// </summary>
        /// <param name="metricName">metric name</param>
        /// <param name="filter">character filter which is applied to the scope components if not null.</param>
        /// <param name="reporterIndex">index of the reporter whose delimiter should be used</param>
        /// <returns>fully qualified metric name</returns>
        public string GetMetricIdentifier(string metricName, ICharacterFilter filter, int reporterIndex)
        {
            var delimiter = Registry.Delimiter;

            if (_scopeStrings.Length == 0 || (reporterIndex < 0 || reporterIndex >= _scopeStrings.Length))
            {
                string newScopeString;
                if (filter != null)
                {
                    newScopeString = ScopeFormat.Concat(filter, delimiter, ScopeComponents);
                    metricName = filter.FilterCharacters(metricName);
                }
                else
                {
                    newScopeString = ScopeFormat.Concat(delimiter, ScopeComponents);
                }

                return newScopeString + delimiter + metricName;
            }
            else
            {
                if (_scopeStrings[reporterIndex] == null)
                {
                    if (filter != null)
                    {
                        _scopeStrings[reporterIndex] = ScopeFormat.Concat(filter, delimiter, ScopeComponents);
                    }
                    else
                    {
                        _scopeStrings[reporterIndex] = ScopeFormat.Concat(delimiter, ScopeComponents);
                    }
                }
                if (filter != null)
                {
                    metricName = filter.FilterCharacters(metricName);
                }

                return _scopeStrings[reporterIndex] + delimiter + metricName;
            }
        }

        public void Close()
        {
            lock (this)
            {
                if (!_closed)
                {
                    _closed = true;

                    // close all subgroups
                    foreach (var @group in _groups.Values)
                        @group.Close();

                    _groups.Clear();

                    // un-register all directly contained metrics
                    foreach (var metric in _metrics)
                        Registry.UnRegister(metric.Value, metric.Key, this);

                    _metrics.Clear();
                }
            }
        }

        public THistogram Histogram<THistogram>(int name, THistogram histogram) where THistogram : IHistogram
        {
            throw new System.NotImplementedException();
        }

        public THistogram Histogram<THistogram>(string name, THistogram histogram) where THistogram : IHistogram
        {
            throw new System.NotImplementedException();
        }

        public TMeter Meter<TMeter>(int name, TMeter meter) where TMeter : IMeter
        {
            throw new System.NotImplementedException();
        }

        public TMeter Meter<TMeter>(string name, TMeter meter) where TMeter : IMeter
        {
            throw new System.NotImplementedException();
        }
    }
}
