namespace FLink.Core.Api.Common
{
    /// <summary>
    /// Possible states of a job once it has been accepted by the job manager.
    /// </summary>
    public class JobStatus
    {
        /// <summary>
        /// Job is newly created, no task has started to run.
        /// </summary>
        public static JobStatus Created = new JobStatus(TerminalState.Globally);

        /// <summary>
        /// Some tasks are scheduled or running, some may be pending, some may be finished.
        /// </summary>
        public static JobStatus Running = new JobStatus(TerminalState.NonTerminal);

        /// <summary>
        /// The job has failed and is currently waiting for the cleanup to complete.
        /// </summary>
        public static JobStatus Failing = new JobStatus(TerminalState.NonTerminal);

        /// <summary>
        /// The job has failed with a non-recoverable task failure.
        /// </summary>
        public static JobStatus Failed = new JobStatus(TerminalState.Globally);

        /// <summary>
        /// Job is being cancelled.
        /// </summary>
        public static JobStatus Cancelling = new JobStatus(TerminalState.NonTerminal);

        /// <summary>
        /// Job has been cancelled.
        /// </summary>
        public static JobStatus Canceled = new JobStatus(TerminalState.Globally);

        /// <summary>
        /// All of the job's tasks have successfully finished.
        /// </summary>
        public static JobStatus Finished = new JobStatus(TerminalState.Globally);

        /// <summary>
        /// The job is currently undergoing a reset and total restart.
        /// </summary>
        public static JobStatus Restarting = new JobStatus(TerminalState.NonTerminal);

        /// <summary>
        /// The job has been suspended which means that it has been stopped but not been removed from a potential HA job store.
        /// </summary>
        public static JobStatus Suspended = new JobStatus(TerminalState.Locally);

        /// <summary>
        /// The job is currently reconciling and waits for task execution report to recover state.
        /// </summary>
        public static JobStatus Reconciling = new JobStatus(TerminalState.NonTerminal);

        public enum TerminalState
        {
            NonTerminal,
            Locally,
            Globally
        }

        private readonly TerminalState _terminalState;

        public JobStatus(TerminalState terminalState) => _terminalState = terminalState;

        /// <summary>
        /// True, if this job status is globally terminal, false otherwise.
        /// Checks whether this state is <i>globally terminal</i>.
        /// A globally terminal job is complete and cannot fail any more and will not be restarted or recovered by another standby master node.
        /// When a globally terminal state has been reached, all recovery data for the job is dropped from the high-availability services.
        /// </summary>
        public bool IsGloballyTerminalState => _terminalState == TerminalState.Globally;

        /// <summary>
        /// True, if this job status is terminal, false otherwise.
        /// Checks whether this state is <i>locally terminal</i>.
        /// Locally terminal refers to the state of a job's execution graph within an executing JobManager.
        /// If the execution graph is locally terminal, the JobManager will not continue executing or recovering the job.
        /// </summary>
        public bool IsTerminalState => _terminalState == TerminalState.NonTerminal;
    }
}
