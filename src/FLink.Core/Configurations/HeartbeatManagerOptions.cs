namespace FLink.Core.Configurations
{
    /// <summary>
    /// The set of configuration options relating to heartbeat manager settings.
    /// </summary>
    public class HeartbeatManagerOptions
    {
        // Not intended to be instantiated.
        private HeartbeatManagerOptions() { }

        public static readonly ConfigOption<long> HeartbeatInterval = ConfigOptionBuilder
            .Key("heartbeat.interval")
            .DefaultValue(10000L)
            .WithDescription("Time interval for requesting heartbeat from sender side.");

        public static readonly ConfigOption<long> HeartbeatTimeout = ConfigOptionBuilder
            .Key("heartbeat.timeout")
            .DefaultValue(50000L)
            .WithDescription("Timeout for requesting and receiving heartbeat for both sender and receiver sides.");
    }
}
