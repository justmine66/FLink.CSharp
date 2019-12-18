using System;
using System.Threading.Tasks;
using FLink.Core.Api.Common;
using FLink.Core.Configurations;
using FLink.Core.Exceptions;
using FLink.Runtime.Checkpoint;
using FLink.Runtime.Execution;
using FLink.Runtime.IO.Network.Api;

namespace FLink.Runtime.JobGraphs.Tasks
{
    /// <summary>
    /// This is the abstract base class for every task that can be executed by a TaskManager.
    /// Concrete tasks extend this class, for example the streaming and batch tasks.
    /// The TaskManager invokes the <see cref="Invoke()"/> method when executing a task. All operations of the task happen in this method (setting up input output stream readers and writers as well as the task's core operation).
    /// NOTE: There is no constructor that accepts and initial task state snapshot and stores it in a variable.That is on purpose, because the AbstractInvokable itself does not need the state snapshot  and we do not want to store a reference indefinitely, thus preventing cleanup of the initial state structure by the Garbage Collector.
    /// </summary>
    public abstract class AbstractInvokable
    {
        /// <summary>
        /// Create an Invokable task and set its environment.
        /// </summary>
        /// <param name="environment">Assigned to this invokable.</param>
        protected AbstractInvokable(IEnvironment environment) => Environment = environment;

        #region [ Core ]

        /// <summary>
        /// Gets and sets whether the thread that executes the <see cref="Invoke"/> method should be interrupted during cancellation.
        /// </summary>
        public bool ShouldInterruptOnCancel { get; set; }

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
        public virtual void Cancel()
        {
            // The default implementation does nothing.
        }

        #endregion

        #region [ Access to Environment and Configuration ]

        /// <summary>
        /// Gets the environment assigned to this task.
        /// </summary>
        public IEnvironment Environment;

        /// <summary>
        /// Gets the user code class type
        /// </summary>
        public Type UserClassType => Environment.UserClassType;

        /// <summary>
        /// Gets the current number of subtasks the respective task is split into.
        /// </summary>
        public int CurrentNumberOfSubTasks => Environment.TaskInfo.NumberOfParallelSubTasks;

        /// <summary>
        /// Gets the index of this subtask in the subtask group.
        /// </summary>
        public int IndexInSubTaskGroup => Environment.TaskInfo.IndexOfSubTask;

        /// <summary>
        /// Gets the task-wide configuration object, originally attached to the job vertex.
        /// </summary>
        public Configuration TaskConfiguration => Environment.TaskConfiguration;

        /// <summary>
        /// Gets the job-wide configuration object that was attached to the JobGraph.
        /// </summary>
        public Configuration JobConfiguration => Environment.JobConfiguration;

        /// <summary>
        /// Gets the global ExecutionConfig.
        /// </summary>
        public ExecutionConfig ExecutionConfig => Environment.ExecutionConfig;

        #endregion

        #region [ Checkpoint Methods ]

        /// <summary>
        /// This method is called to trigger a checkpoint, asynchronously by the checkpoint coordinator.
        /// This method is called for tasks that start the checkpoints by injecting the initial barriers i.e., the source tasks. In contrast, checkpoints on downstream operators, which are the result of receiving checkpoint barriers, invoke the <see cref="TriggerCheckpointOnBarrier"/>.
        /// </summary>
        /// <param name="checkpointMetaData">For about this checkpoint</param>
        /// <param name="checkpointOptions">For performing this checkpoint</param>
        /// <param name="advanceToEndOfEventTime">Flag indicating if the source should inject a max watermark in the pipeline to fire any registered event-time timers.</param>
        /// <returns>future with value of false if the checkpoint was not carried out, true otherwise</returns>
        public virtual TaskCompletionSource<bool> TriggerCheckpointAsync(
        CheckpointMetaData checkpointMetaData,
        CheckpointOptions checkpointOptions,
        bool advanceToEndOfEventTime) => throw new UnSupportedOperationException($"{nameof(TriggerCheckpointAsync)} not supported by {GetType().Name}");

        /// <summary>
        /// This method is called when a checkpoint is triggered as a result of receiving checkpoint barriers on all input streams.
        /// </summary>
        /// <param name="checkpointMetaData">Meta data for about this checkpoint</param>
        /// <param name="checkpointOptions">Options for performing this checkpoint</param>
        /// <param name="checkpointMetrics">Metrics about this checkpoint</param>
        /// <exception cref="Exception">Exceptions thrown as the result of triggering a checkpoint are forwarded.</exception>
        public virtual void TriggerCheckpointOnBarrier(CheckpointMetaData checkpointMetaData, CheckpointOptions checkpointOptions, CheckpointMetrics checkpointMetrics)
            => throw new UnSupportedOperationException($"{nameof(TriggerCheckpointOnBarrier)} not supported by {GetType().Name}");

        /// <summary>
        /// Aborts a checkpoint as the result of receiving possibly some checkpoint barriers, but at least one <see cref="CancelCheckpointMarker"/>.
        /// This requires implementing tasks to forward a <see cref="CancelCheckpointMarker"/> to their outputs. 
        /// </summary>
        /// <param name="checkpointId">The ID of the checkpoint to be aborted.</param>
        /// <param name="cause">The reason why the checkpoint was aborted during alignment.</param>
        public void AbortCheckpointOnBarrier(long checkpointId, Exception cause)
            => throw new UnSupportedOperationException($"{nameof(AbortCheckpointOnBarrier)} not supported by {GetType().Name}");

        /// <summary>
        /// Invoked when a checkpoint has been completed, i.e., when the checkpoint coordinator has received the notification from all participating tasks.
        /// </summary>
        /// <param name="checkpointId">The ID of the checkpoint that is complete.</param>
        /// <returns> that completes when the notification has been processed by the task.</returns>
        public TaskCompletionSource<bool> NotifyCheckpointCompleteAsync(long checkpointId)
            => throw new UnSupportedOperationException($"{nameof(NotifyCheckpointCompleteAsync)} not supported by {GetType().Name}");

        #endregion
    }
}
