using System;
using System.Collections.Generic;
using FLink.Core.Api.Common.Accumulators;
using FLink.Core.Api.Common.Cache;
using FLink.Core.Api.Common.State;
using FLink.Metrics.Core;

namespace FLink.Core.Api.Common.Functions
{
    /// <summary>
    /// A RuntimeContext contains information about the context in which functions are executed.
    /// Each parallel instance of the function will have a context through which it can access static contextual information(such as the current parallelism) and other constructs like accumulators and broadcast variables.
    /// </summary>
    public interface IRuntimeContext
    {
        /// <summary>
        /// Gets the name of the task in which the UDF runs, as assigned during plan construction.
        /// </summary>
        string TaskName { get; }

        /// <summary>
        /// Gets the metric group for this parallel sub task.
        /// </summary>
        IMetricGroup MetricGroup { get; }

        /// <summary>
        /// Gets the parallelism with which the parallel task runs.
        /// </summary>
        int NumberOfParallelSubTasks { get; }

        /// <summary>
        /// Gets the number of max-parallelism with which the parallel task runs.
        /// </summary>
        int MaxNumberOfParallelSubTasks { get; }

        /// <summary>
        /// Gets the number of this parallel subTask. The numbering starts from 0 and goes up to parallelism-1(parallelism as returned by <see cref="NumberOfParallelSubTasks"/>)..
        /// </summary>
        int IndexOfThisSubTask { get; }

        /// <summary>
        /// Gets the attempt number of this parallel sub task. First attempt is numbered 0.
        /// </summary>
        int AttemptNumber { get;  }

        /// <summary>
        /// Gets the name of the task, appended with the sub task indicator, such as "MyTask (3/6)", where 3 would be(<see cref="IndexOfThisSubTask"/> + 1), and 6 would be <see cref="NumberOfParallelSubTasks"/>.
        /// </summary>
        string TaskNameWithSubTasks { get;  }

        /// <summary>
        /// Gets the <see cref="ExecutionConfig"/> for the currently executing
        /// </summary>
        ExecutionConfig ExecutionConfig { get;  }

        /// <summary>
        /// Add this accumulator. Throws an exception if the accumulator already exists in the same Task. 
        /// Note that the Accumulator name must have an unique name across the FLink job. Otherwise you will get an error when incompatible accumulators from different Tasks are combined at the JobManager upon job completion.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="name"></param>
        /// <param name="accumulator"></param>
        void AddAccumulator<TValue, TResult>(string name, IAccumulator<TValue, TResult> accumulator);

        /// <summary>
        /// Get an existing accumulator object. The accumulator must have been added previously in this local runtime context.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        IAccumulator<TValue, TResult> GetAccumulator<TValue, TResult>(string name);

        /// <summary>
        /// Returns a map of all registered accumulators for this task. The returned map must not be modified.
        /// </summary>
        /// <returns></returns>
        IReadOnlyDictionary<string, IAccumulator<dynamic, dynamic>> AllAccumulators { get; }

        /// <summary>
        /// Convenience function to create a counter object for integers.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IntCounter GetIntCounter(string name);

        /// <summary>
        /// Convenience function to create a counter object for longs.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        LongCounter GetLongCounter(string name);

        /// <summary>
        /// Convenience function to create a counter object for doubles.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        DoubleCounter GetDoubleCounter(string name);

        /// <summary>
        /// Convenience function to create a counter object for histograms.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IHistogram GetHistogram(string name);

        /// <summary>
        /// Tests for the existence of the broadcast variable identified by the  given name.
        /// </summary>
        /// <param name="name">The name under which the broadcast variable is registered;</param>
        /// <returns>Whether a broadcast variable exists for the given name.</returns>
        bool HasBroadcastVariable(string name);

        /// <summary>
        /// Returns the result bound to the broadcast variable identified by the given name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        List<T> GetBroadcastVariable<T>(string name);

        /// <summary>
        /// Get the local temporary file copies of files otherwise not locally accessible.
        /// The distributed cache of the worker executing this instance.
        /// </summary>
        DistributedCache DistributedCache { get; }

        #region [ Methods for accessing state ]

        /// <summary>
        /// Gets a handle to the system's key/value state. The key/value state is only accessible if the function is executed on a KeyedStream. On each access, the state exposes the value for the key of the element currently processed by the function.
        /// Each function may have multiple partitioned states, addressed with different names.
        /// Because the scope of each value is the key of the currently processed element, and the elements are distributed by the Flink runtime, the system can transparently scale out and redistribute the state and KeyedStream.
        /// </summary>
        /// <typeparam name="TValue">The type of value stored in the state.</typeparam>
        /// <param name="stateProperties">The descriptor defining the properties of the stats.</param>
        /// <returns>The partitioned state object.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if no partitioned state is available for the function (function is not part os a KeyedStream).</exception>
        IValueState<TValue> GetState<TValue>(ValueStateDescriptor<TValue> stateProperties);

        /// <summary>
        /// Gets a handle to the system's key/value list state. This state is similar to the state accessed via <see cref="GetState{TValue}"/>, but is optimized for state that holds lists. One can add elements to the list, or retrieve the list as a whole.
        /// This state is only accessible if the function is executed on a KeyedStream.
        /// </summary>
        /// <typeparam name="TValue">The type of value stored in the state.</typeparam>
        /// <param name="stateProperties">The descriptor defining the properties of the stats.</param>
        /// <returns>The partitioned state object.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if no partitioned state is available for the function (function is not part os a KeyedStream).</exception>
        IListState<TValue> GetListState<TValue>(ListStateDescriptor<TValue> stateProperties);

        /// <summary>
        /// Gets a handle to the system's key/value reducing state. This state is similar to the state accessed via <see cref="GetState{TValue}"/>, but is optimized for state that aggregates values.
        /// This state is only accessible if the function is executed on a KeyedStream.
        /// </summary>
        /// <typeparam name="TValue">The type of value stored in the state.</typeparam>
        /// <param name="stateProperties">The descriptor defining the properties of the stats.</param>
        /// <returns>The partitioned state object.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if no partitioned state is available for the function (function is not part os a KeyedStream).</exception>
        IReducingState<TValue> GetReducingState<TValue>(ReducingStateDescriptor<TValue> stateProperties);

        /// <summary>
        /// Gets a handle to the system's key/value aggregating state. This state is similar to the state accessed via <see cref="GetState{TValue}"/>, but is optimized for state that aggregates values with different types.
        /// This state is only accessible if the function is executed on a KeyedStream.
        /// </summary>
        /// <typeparam name="TInput">The type of the values that are added to the state.</typeparam>
        /// <typeparam name="TAccumulator">The type of the accumulator (intermediate aggregation state).</typeparam>
        /// <typeparam name="TOutput">The type of the values that are returned from the state.</typeparam>
        /// <param name="stateProperties">The descriptor defining the properties of the stats.</param>
        /// <returns>The partitioned state object.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if no partitioned state is available for the function (function is not part os a KeyedStream).</exception>
        IAggregatingState<TInput, TOutput> GetAggregatingState<TInput, TAccumulator, TOutput>(AggregatingStateDescriptor<TInput, TAccumulator, TOutput> stateProperties);

        /// <summary>
        /// Gets a handle to the system's key/value map state. This state is similar to the state accessed via <see cref="GetState{TValue}"/>, but is optimized for state that is composed of user-defined key-value pairs.
        /// This state is only accessible if the function is executed on a KeyedStream.
        /// </summary>
        /// <typeparam name="TKey">The type of the user keys stored in the state.</typeparam>
        /// <typeparam name="TValue">The type of the user values stored in the state.</typeparam>
        /// <param name="stateProperties">The descriptor defining the properties of the stats.</param>
        /// <returns>The partitioned state object.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if no partitioned state is available for the function (function is not part os a KeyedStream).</exception>
        IMapState<TKey, TValue> GetMapState<TKey, TValue>(MapStateDescriptor<TKey, TValue> stateProperties);

        #endregion
    }
}
