using FLink.Core.Api.Common.Accumulators;
using FLink.Metrics.Core;
using System.Collections.Generic;

namespace FLink.Core.Api.Common.Functions
{
    /// <summary>
    /// A RuntimeContext contains information about the context in which functions are executed.
    /// Each parallel instance of the function will have a context through which it can access static contextual information(such as the current parallelism) and other constructs like accumulators and broadcast variables.
    /// </summary>
    public interface IRuntimeContext
    {
        /// <summary>
        /// Returns the name of the task in which the UDF runs, as assigned during plan construction.
        /// </summary>
        string TaskName { get; set; }

        /// <summary>
        /// Returns the metric group for this parallel subtask.
        /// </summary>
        IMetricGroup MetricGroup { get; set; }

        /// <summary>
        /// Gets the parallelism with which the parallel task runs.
        /// </summary>
        int NumberOfParallelSubtasks { get; set; }

        /// <summary>
        /// Gets the number of max-parallelism with which the parallel task runs.
        /// </summary>
        int MaxNumberOfParallelSubtasks { get; set; }

        /// <summary>
        /// Gets the number of this parallel subtask. The numbering starts from 0 and goes up to parallelism-1(parallelism as returned by <see cref="NumberOfParallelSubtasks"/>)..
        /// </summary>
        int IndexOfThisSubtask { get; set; }

        /// <summary>
        /// Gets the attempt number of this parallel subtask. First attempt is numbered 0.
        /// </summary>
        int AttemptNumber { get; set; }

        /// <summary>
        /// Returns the name of the task, appended with the subtask indicator, such as "MyTask (3/6)", where 3 would be(<see cref="IndexOfThisSubtask"/> + 1), and 6 would be <see cref="NumberOfParallelSubtasks"/>.
        /// </summary>
        string TaskNameWithSubtasks { get; set; }

        /// <summary>
        /// Returns the <see cref="ExecutionConfig"/> for the currently executing
        /// </summary>
        ExecutionConfig ExecutionConfig { get; set; }

        /// <summary>
        /// Add this accumulator. Throws an exception if the accumulator already exists in the same Task. 
        /// Note that the Accumulator name must have an unique name across the Flink job. Otherwise you will get an error when incompatible accumulators from different Tasks are combined at the JobManager upon job completion.
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
        IReadOnlyDictionary<string, IAccumulator<dynamic, dynamic>> GetAllAccumulators();

        /// <summary>
        /// Convenience function to create a counter object for integers.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IntCounter GetIntCounter(string name);

        LongCounter GetLongCounter(string name);

        DoubleCounter GetDoubleCounter(string name);

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
    }
}
