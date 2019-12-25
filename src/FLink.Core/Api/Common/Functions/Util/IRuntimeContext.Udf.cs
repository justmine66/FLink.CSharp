using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FLink.Core.Api.Common.Accumulators;
using FLink.Core.Api.Common.Cache;
using FLink.Core.Api.Common.States;
using FLink.Core.Exceptions;
using FLink.Core.Util;
using FLink.Metrics.Core;

namespace FLink.Core.Api.Common.Functions.Util
{
    /// <summary>
    /// A standalone implementation of the <see cref="IRuntimeContext"/>, created by runtime UDF operators.
    /// </summary>
    public abstract class AbstractRuntimeUdfContext : IRuntimeContext
    {
        private readonly TaskInfo _taskInfo;
        private readonly IDictionary<string, IAccumulator<object, object>> _accumulators;

        protected AbstractRuntimeUdfContext(
            TaskInfo taskInfo,
            ExecutionConfig executionConfig,
            IDictionary<string, IAccumulator<object, object>> accumulators,
            IDictionary<string, TaskCompletionSource<string>> cpTasks,
            IMetricGroup metrics)
        {
            _taskInfo = taskInfo;
            ExecutionConfig = executionConfig;
            DistributedCache = new DistributedCache(cpTasks);
            _accumulators = Preconditions.CheckNotNull(accumulators);
            MetricGroup = metrics;
        }

        public DistributedCache DistributedCache { get; }
        public string TaskName => _taskInfo.TaskName;
        public IMetricGroup MetricGroup { get; }
        public int NumberOfParallelSubTasks => _taskInfo.NumberOfParallelSubTasks;
        public int MaxNumberOfParallelSubTasks => _taskInfo.MaxNumberOfParallelSubTasks;
        public int IndexOfThisSubTask => _taskInfo.IndexOfSubTask;
        public int AttemptNumber => _taskInfo.AttemptNumber;
        public string TaskNameWithSubTasks => _taskInfo.TaskNameWithSubTasks;
        public ExecutionConfig ExecutionConfig { get; }
        public IReadOnlyDictionary<string, IAccumulator<object, object>> AllAccumulators =>
            new ReadOnlyDictionary<string, IAccumulator<object, object>>(_accumulators);

        public virtual void AddAccumulator<TValue, TResult>(string name, IAccumulator<TValue, TResult> accumulator)
        {
            if (_accumulators.ContainsKey(name))
            {
                throw new InvalidOperationException($"The accumulator '{name}' already exists and cannot be added.");
            }

            _accumulators.Add(name, accumulator as IAccumulator<object, object>);
        }

        public virtual IAccumulator<TValue, TResult> GetAccumulator<TValue, TResult>(string name)
        {
            throw new System.NotImplementedException();
        }

        public virtual IntCounter GetIntCounter(string name) => GetAccumulator<int, int>(name) as IntCounter;

        public virtual LongCounter GetLongCounter(string name) => GetAccumulator<long, long>(name) as LongCounter;

        public virtual DoubleCounter GetDoubleCounter(string name) => GetAccumulator<double, double>(name) as DoubleCounter;

        public virtual IHistogram GetHistogram(string name)
        {
            throw new System.NotImplementedException();
        }

        public virtual bool HasBroadcastVariable(string name)
        {
            throw new System.NotImplementedException();
        }

        public virtual List<T> GetBroadcastVariable<T>(string name)
        {
            throw new System.NotImplementedException();
        }

        public virtual IValueState<TValue> GetState<TValue>(ValueStateDescriptor<TValue> stateProperties)
        {
            throw new UnSupportedOperationException("This state is only accessible by functions executed on a KeyedStream");
        }

        public virtual IListState<TValue> GetListState<TValue>(ListStateDescriptor<TValue> stateProperties)
        {
            throw new UnSupportedOperationException("This state is only accessible by functions executed on a KeyedStream");
        }

        public virtual IReducingState<TValue> GetReducingState<TValue>(ReducingStateDescriptor<TValue> stateProperties)
        {
            throw new UnSupportedOperationException("This state is only accessible by functions executed on a KeyedStream");
        }

        public virtual IAggregatingState<TInput, TOutput> GetAggregatingState<TInput, TAccumulator, TOutput>(AggregatingStateDescriptor<TInput, TAccumulator, TOutput> stateProperties)
        {
            throw new UnSupportedOperationException("This state is only accessible by functions executed on a KeyedStream");
        }

        public virtual IMapState<TKey, TValue> GetMapState<TKey, TValue>(MapStateDescriptor<TKey, TValue> stateProperties)
        {
            throw new UnSupportedOperationException("This state is only accessible by functions executed on a KeyedStream");
        }

        internal string AllocationIdAsString => _taskInfo.AllocationIdAsString;
    }
}
