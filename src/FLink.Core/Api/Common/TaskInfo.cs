namespace FLink.Core.Api.Common
{
    using static FLink.Core.Util.Preconditions;

    /// <summary>
    /// Encapsulates task-specific information: name, index of subtask, parallelism and attempt number.
    /// </summary>
    public class TaskInfo
    {
        /// <summary>
        /// Returns the name of the task
        /// </summary>
        public string TaskName;
        /// <summary>
        /// The name of the task, with subtask indicator, such as "MyTask (3/6)", where 3 would be (<see cref="IndexOfSubTask"/> + 1), and 6 would be <see cref="NumberOfParallelSubTasks"/>.
        /// </summary>
        public string TaskNameWithSubTasks;
        /// <summary>
        /// Returns the allocation id for where this task is executed.
        /// </summary>
        public string AllocationIdAsString;
        /// <summary>
        /// Gets the max parallelism aka the max number of subtasks.
        /// </summary>
        public int MaxNumberOfParallelSubTasks;
        /// <summary>
        /// Gets the number of this parallel subtask. The numbering starts from 0 and goes up to <see cref="MaxNumberOfParallelSubTasks"/> - 1.
        /// </summary>
        public int IndexOfSubTask;
        /// <summary>
        /// Gets the parallelism with which the parallel task runs.
        /// </summary>
        public int NumberOfParallelSubTasks;
        /// <summary>
        /// Gets the attempt number of this parallel subtask. First attempt is numbered 0. The attempt number corresponds to the number of times this task has been restarted(after failure/cancellation) since the job was initially started.
        /// </summary>
        public int AttemptNumber;

        public TaskInfo(
            string taskName,
            int maxNumberOfParallelSubTasks,
            int indexOfSubTask,
            int numberOfParallelSubTasks,
            int attemptNumber)
            : this(
                taskName,
                maxNumberOfParallelSubTasks,
                indexOfSubTask,
                numberOfParallelSubTasks,
                attemptNumber,
                "UNKNOWN")
        { }

        public TaskInfo(
            string taskName,
            int maxNumberOfParallelSubTasks,
            int indexOfSubTask,
            int numberOfParallelSubTasks,
            int attemptNumber,
            string allocationIdAsString)
        {
            CheckArgument(indexOfSubTask >= 0, "Task index must be a non-negative number.");
            CheckArgument(maxNumberOfParallelSubTasks >= 1, "Max parallelism must be a positive number.");
            CheckArgument(maxNumberOfParallelSubTasks >= numberOfParallelSubTasks, "Max parallelism must be >= than parallelism.");
            CheckArgument(numberOfParallelSubTasks >= 1, "Parallelism must be a positive number.");
            CheckArgument(indexOfSubTask < numberOfParallelSubTasks, "Task index must be less than parallelism.");
            CheckArgument(attemptNumber >= 0, "Attempt number must be a non-negative number.");

            TaskName = CheckNotNull(taskName, "Task Name must not be null.");
            MaxNumberOfParallelSubTasks = maxNumberOfParallelSubTasks;
            IndexOfSubTask = indexOfSubTask;
            NumberOfParallelSubTasks = numberOfParallelSubTasks;
            AttemptNumber = attemptNumber;
            TaskNameWithSubTasks = taskName + " (" + (indexOfSubTask + 1) + '/' + numberOfParallelSubTasks + ')';
            AllocationIdAsString = CheckNotNull(allocationIdAsString);
        }
    }
}
