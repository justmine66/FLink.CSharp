namespace FLink.Runtime.JobGraphs
{
    /// <summary>
    /// Savepoint restore settings.
    /// </summary>
    public class SavepointRestoreSettings
    {
        public static readonly SavepointRestoreSettings None = new SavepointRestoreSettings(null, false);

        /// <summary>
        /// Creates the restore settings.
        /// </summary>
        /// <param name="restorePath">Savepoint restore path.</param>
        /// <param name="allowNonRestoredState">Ignore unmapped state.</param>
        public SavepointRestoreSettings(string restorePath, bool allowNonRestoredState)
        {
            RestorePath = restorePath;
            AllowNonRestoredState = allowNonRestoredState;
        }

        /// <summary>
        /// Gets the path to the savepoint to restore from.
        /// </summary>
        public string RestorePath { get; }

        /// <summary>
        /// Gets whether non restored state is allowed if the savepoint contains state that cannot be mapped back to the job.
        /// </summary>
        public bool AllowNonRestoredState { get; }

        /// <summary>
        /// Gets whether to restore from savepoint.
        /// </summary>
        public bool RestoreSavepoint => RestorePath != null;
    }
}
