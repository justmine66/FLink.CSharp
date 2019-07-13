using System.Collections.Generic;
using FLink.Core.Api.Common;
using FLink.Streaming.Api.Transformations;

namespace FLink.Streaming.Api.Environment
{
    /// <summary>
    /// The StreamExecutionEnvironment is the context in which a streaming program is executed. 
    /// </summary>
    /// <remarks>
    /// The environment provides methods to control the job execution (such as setting the parallelism or the fault tolerance/check pointing parameters) and to interact with the outside world(data access).
    /// </remarks>
    public abstract class StreamExecutionEnvironment
    {
        // The default name to use for a streaming job if no other name has been specified.
        public const string DefaultJobName = "Flink Streaming Job";

        // The time characteristic that is used if none other is set.
        private const TimeCharacteristic DefaultTimeCharacteristic = TimeCharacteristic.ProcessingTime;

        // The default buffer timeout (max delay of records in the network stack).
        private const long DefaultNetworkBufferTimeout = 100L;

        // The environment of the context (local by default, cluster if invoked through command line).
        private static IStreamExecutionEnvironmentFactory _contextEnvironmentFactory;

        // The default parallelism used when creating a local environment.
        private static int _defaultLocalParallelism = System.Environment.ProcessorCount;

        // The execution configuration for this environment.
        private static ExecutionConfig _config = new ExecutionConfig();

        private readonly List<StreamTransformation<object>> _transformations = new List<StreamTransformation<object>>();

        private long _bufferTimeout = DefaultNetworkBufferTimeout;

        protected bool IsChainingEnabled = true;

        public CheckpointConfig CheckpointConfig { get; private set; }

        /// <summary>
        /// Enables checkpointing for the streaming job.
        /// The distributed state of the streaming data flow will be periodically snapshotted. In case of a failure, the streaming data flow will be restarted from the latest completed checkpoint.
        /// </summary>
        /// <param name="interval">interval Time interval between state checkpoints in milliseconds.</param>
        /// <returns></returns>
        public StreamExecutionEnvironment EnableCheckPointing(long interval)
        {
            CheckpointConfig.CheckpointInterval = interval;
            return this;
        }

        public StreamExecutionEnvironment EnableCheckPointing(long interval, CheckPointingMode mode)
        {
            CheckpointConfig.CheckpointInterval = interval;
            CheckpointConfig.CheckPointingMode = mode;
            return this;
        }

        public StreamExecutionEnvironment EnableCheckPointing(long interval, CheckPointingMode mode, bool force)
        {
            CheckpointConfig.CheckpointInterval = interval;
            CheckpointConfig.CheckPointingMode = mode;
            CheckpointConfig.ForceCheckPointing = force;
            return this;
        }

        public StreamExecutionEnvironment EnableCheckPointing()
        {
            CheckpointConfig.CheckpointInterval = 500;
            return this;
        }
    }
}
