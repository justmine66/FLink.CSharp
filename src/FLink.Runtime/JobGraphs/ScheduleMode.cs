namespace FLink.Runtime.JobGraphs
{
    /// <summary>
    /// The ScheduleMode decides how tasks of an execution graph are started.
    /// </summary>
    public class ScheduleMode
    {
        /// <summary>
        /// Schedule tasks lazily from the sources. Downstream tasks are started once their input data are ready.
        /// </summary>
        public static readonly ScheduleMode LazyFromSources = new ScheduleMode(true);

        /// <summary>
        /// Same as <see cref="LazyFromSources"/> just with the difference that it uses batch slot requests which support the execution of jobs with fewer slots than requested. However, the user needs to make sure that the job does not contain any pipelined shuffles (every pipelined region can be executed with a single slot).
        /// </summary>
        public static readonly ScheduleMode LazyFromSourcesWithBatchSlotRequest = new ScheduleMode(true);

        /// <summary>
        /// Schedules all tasks immediately.
        /// </summary>
        public static readonly ScheduleMode Eager = new ScheduleMode(false);

        public ScheduleMode(bool allowLazyDeployment) => AllowLazyDeployment = allowLazyDeployment;

        /// <summary>
        /// Gets whether we are allowed to deploy consumers lazily.
        /// </summary>
        public bool AllowLazyDeployment { get; }
    }
}
