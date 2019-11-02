namespace FLink.Core.Configurations
{
    /// <summary>
    /// A collection of all configuration options that relate to checkpoints and savepoints.
    /// </summary>
    public class CheckpointingOptions
    {
        public static readonly ConfigOption<string> CheckpointsDirectory = ConfigOptionBuilder
            .Key("state.checkpoints.dir")
            .NoDefaultValue()
            .WithDeprecatedKeys("state.backend.fs.checkpointdir")
            .WithDescription("The default directory used for storing the data files and meta data of checkpoints " +
            "in a Flink supported filesystem. The storage path must be accessible from all participating processes/nodes" +
            "(i.e. all TaskManagers and JobManagers).");

        public static readonly ConfigOption<bool> AsyncSnapshots = ConfigOptionBuilder
            .Key("state.backend.async")
            .DefaultValue(true)
            .WithDescription("Option whether the state backend should use an asynchronous snapshot method where" +
            " possible and configurable. Some state backends may not support asynchronous snapshots, or only support" +
            " asynchronous snapshots, and ignore this option.");
    }
}
