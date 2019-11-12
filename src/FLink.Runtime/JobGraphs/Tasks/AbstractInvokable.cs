using System;
using System.Threading.Tasks;
using FLink.Core.Api.Common;
using FLink.Core.Configurations;
using FLink.Runtime.Checkpoint;
using FLink.Runtime.Execution;

namespace FLink.Runtime.JobGraphs.Tasks
{
    /// <summary>
    /// This is the abstract base class for every task that can be executed by a TaskManager.
    /// Concrete tasks extend this class, for example the streaming and batch tasks.
    /// </summary>
    public abstract class AbstractInvokable
    {
        /// <summary>
        /// Gets the environment assigned to this task.
        /// </summary>
        public IEnvironment Environment;

        /// <summary>
        /// Gets the current number of subtasks the respective task is split into.
        /// </summary>
        public int CurrentNumberOfSubTasks => Environment.TaskInfo.NumberOfParallelSubTasks;

        /// <summary>
        /// Gets the index of this subtask in the subtask group.
        /// </summary>
        public int IndexInSubTaskGroup => Environment.TaskInfo.IndexOfSubTask;

        /// <summary>
        /// Gets the global ExecutionConfig.
        /// </summary>
        public ExecutionConfig ExecutionConfig => Environment.ExecutionConfig;

        /// <summary>
        /// Gets the task-wide configuration object, originally attached to the job vertex.
        /// </summary>
        Configuration TaskConfiguration => Environment.TaskConfiguration;

        /// <summary>
        /// Gets the job-wide configuration object that was attached to the JobGraph.
        /// </summary>
        Configuration JobConfiguration => Environment.JobConfiguration;

        /// <summary>
        /// Gets and sets whether the thread that executes the <see cref="Invoke"/> method should be interrupted during cancellation.
        /// </summary>
        public bool ShouldInterruptOnCancel { get; set; }

        /// <summary>
        /// Create an Invokable task and set its environment.
        /// </summary>
        /// <param name="environment">Assigned to this invokable.</param>
        protected AbstractInvokable(IEnvironment environment) => Environment = environment;

        #region [ Core Methods ]

        /// <summary>
        /// Starts the execution.
        /// Must be overwritten by the concrete task implementation.
        /// This method is called by the task manager when the actual execution of the task starts.
        /// All resources should be cleaned up when the method returns.
        /// Make sure to guard the code with <code>try-finally</code> blocks where necessary.
        /// </summary>
        /// <exception cref="Exception">Tasks may forward their exceptions for the TaskManager to handle through failure/recovery.</exception>
        public abstract void Invoke();

        /// <summary>
        /// This method is called when a task is canceled either as a result of a user abort or an execution failure.
        /// It can be overwritten to respond to shut down the user code properly.
        /// </summary>
        /// <exception cref="Exception">thrown if any exception occurs during the execution of the user code.</exception>
        public void Cancel() { }

        #endregion

        #region [ Checkpoint Methods ]

        /// <summary>
        /// This method is called to trigger a checkpoint, asynchronously by the checkpoint coordinator.
        /// This method is called for tasks that start the checkpoints by injecting the initial barriers.
        /// </summary>
        /// <param name="checkpointMetaData">For about this checkpoint</param>
        /// <param name="checkpointOptions">For performing this checkpoint</param>
        /// <param name="advanceToEndOfEventTime">Flag indicating if the source should inject a {@code MAX_WATERMARK} in the pipeline to fire any registered event-time timers.</param>
        /// <returns>future with value of false if the checkpoint was not carried out, true otherwise</returns>
        public TaskCompletionSource<bool> TriggerCheckpointAsync(
            CheckpointMetaData checkpointMetaData,
            CheckpointOptions checkpointOptions,
            bool advanceToEndOfEventTime) => throw new InvalidOperationException($"triggerCheckpointAsync not supported by {this.GetType().Name}");

        #endregion
    }
}
