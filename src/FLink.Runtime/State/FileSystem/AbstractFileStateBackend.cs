using FLink.Core.Configurations;
using FLink.Core.Exceptions;

namespace FLink.Runtime.State.FileSystem
{
    /// <summary>
    /// A base class for all state backends that store their metadata (and data) in files.
    /// This class takes the base checkpoint- and savepoint directory paths, but also accepts null for both of then, in which case creating externalized checkpoint is not possible, and it is not possible to create a savepoint with a default path. Null is accepted to enable implementations that only optionally support default savepoints and externalized checkpoints.
    /// </summary>
    public abstract class AbstractFileStateBackend : AbstractStateBackend
    {
        /// <summary>
        /// Gets the checkpoint base directory.
        /// Jobs will create job-specific subdirectories for checkpoints within this directory. May be null, if not configured.
        /// </summary>
        public FsPath BaseCheckpointPath;

        /// <summary>
        /// Gets default directory for savepoints, or null, if no default directory has been configured.
        /// The directory where savepoints are stored by default (when no custom path is given to the savepoint trigger command).
        /// </summary>
        public FsPath BaseSavepointPath;

        /// <summary>
        /// Creates a backend with the given optional checkpoint- and savepoint base directories.
        /// </summary>
        /// <param name="baseCheckpointPath">The base directory for checkpoints, or null, if none is configured.</param>
        /// <param name="baseSavepointPath">The default directory for savepoints, or null, if none is set.</param>
        protected AbstractFileStateBackend(FsPath baseCheckpointPath = default, FsPath baseSavepointPath = default)
        {
            BaseCheckpointPath = baseCheckpointPath == null ? null : ValidatePath(baseCheckpointPath);
            BaseSavepointPath = baseSavepointPath == null ? null : ValidatePath(baseSavepointPath);
        }

        protected AbstractFileStateBackend(FsPath baseCheckpointPath, FsPath baseSavepointPath, Configuration configuration)
        {
            BaseCheckpointPath = baseCheckpointPath == null ? null : ParameterOrConfigured(baseCheckpointPath, configuration, CheckpointingOptions.CheckpointsDirectory);
            BaseSavepointPath = baseSavepointPath == null ? null : ValidatePath(baseSavepointPath);
        }

        #region [ Utilities ]

        private static FsPath ValidatePath(FsPath path)
        {
            var scheme = path.Scheme;
            var pathPart = path.Path;

            // some validity checks
            if (scheme == null)
                throw new IllegalArgumentException("The scheme (hdfs://, file://, etc) is null. " +
                                                   "Please specify the file system scheme explicitly in the URI.");
            if (pathPart == null)
                throw new IllegalArgumentException("The path to store the checkpoint data in is null. " +
                                                   "Please specify a directory path for the checkpoint data.");
            if (pathPart.Length == 0 || pathPart.Equals("/"))
                throw new IllegalArgumentException("Cannot use the root directory for checkpoints.");

            return path;
        }

        private static FsPath ParameterOrConfigured(FsPath path, Configuration config, ConfigOption<string> option)
        {
            if (path != null) return path;

            var configValue = config.GetString(option);
            try
            {
                return configValue == null ? null : new FsPath(configValue);
            }
            catch (IllegalArgumentException)
            {
                throw new IllegalConfigurationException("Cannot parse value for " + option.Key +
                                                        " : " + configValue + " . Not a valid path.");
            }
        }

        #endregion
    }
}
