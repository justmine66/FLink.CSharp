namespace FLink.Runtime.JobManagers
{
    /// <summary>
    /// The startup mode for the JobManager.
    /// </summary>
    public enum JobManagerMode
    {
        /// <summary>
        /// Causes the JobManager to operate in single user mode and start a local embedded TaskManager.
        /// </summary>
        Local,
        /// <summary>
        /// Starts the JobManager in the regular mode where it waits for external TaskManagers to connect.
        /// </summary>
        Cluster
    }
}
