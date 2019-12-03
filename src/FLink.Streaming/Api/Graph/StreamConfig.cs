using FLink.Core.Configurations;
using FLink.Streaming.Api.Operators;

namespace FLink.Streaming.Api.Graph
{
    /// <summary>
    /// Internal configuration for a <see cref="IStreamOperator{TOutput}"/>. This is created and populated by the <see cref="StreamGraphGenerator"/>.
    /// </summary>
    public class StreamConfig
    {
        private static class Constants
        {
            public const string BufferTimeout = "bufferTimeout";
            public const long DefaultTimeout = 100;
            public const string CheckpointingEnabled = "checkpointing";
        }



        public Configuration Config { get; }

        public StreamConfig(Configuration config)
        {
            Config = config;
        }

        public bool IsCheckpointingEnabled => Config.GetBool(Constants.CheckpointingEnabled, false);

        public CheckpointingMode CheckpointMode;

        public long BufferTimeout => Config.GetLong(Constants.BufferTimeout, Constants.DefaultTimeout);
    }
}
