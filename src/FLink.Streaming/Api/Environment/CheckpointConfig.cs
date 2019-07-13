using FLink.Core.Util;

namespace FLink.Streaming.Api.Environment
{
    /// <summary>
    /// Configuration that captures all checkpointing related settings.
    /// </summary>
    public class CheckpointConfig
    {
        /// <summary>
        /// The default checkpoint mode: exactly once.
        /// </summary>
        public static readonly CheckPointingMode DefaultMode = CheckPointingMode.ExactlyOnce;

        /// <summary>
        /// The default timeout of a checkpoint attempt: 10 minutes.
        /// </summary>
        public static readonly long DefaultTimeout = 10 * 60 * 1000;

        /// <summary>
        /// The default minimum pause to be made between checkpoints: none.
        /// </summary>
        public static readonly long DefaultMinPauseBetweenCheckpoints = 0;

        /// <summary>
        /// The default limit of concurrently happening checkpoints: one.
        /// </summary>
        public static readonly int DefaultMaxConcurrentCheckpoints = 1;

        private CheckPointingMode _checkPointingMode = DefaultMode;
        /// <summary>
        /// CheckPointing mode (exactly-once vs. at-least-once).
        /// </summary>
        public CheckPointingMode CheckPointingMode
        {
            get => _checkPointingMode;
            set => _checkPointingMode = Assert.NotNull(value);
        }

        private long _checkpointInterval = -1; // disabled
        /// <summary>
        /// Periodic checkpoint triggering interval.
        /// </summary>
        public long CheckpointInterval
        {
            get => _checkpointInterval;
            set => _checkpointInterval = Assert.Positive(value);
        }

        /// <summary>
        /// Checks whether check pointing is enabled.
        /// </summary>
        /// <returns></returns>
        public bool IsCheckPointingEnabled => _checkpointInterval > 0;

        private long _checkpointTimeout = DefaultTimeout;
        /// <summary>
        /// Maximum time checkpoint may take before being discarded.
        /// </summary>
        public long CheckpointTimeout
        {
            get => _checkpointTimeout;
            set => _checkpointTimeout = Assert.Positive(value);
        }

        private long _minPauseBetweenCheckpoints = DefaultMinPauseBetweenCheckpoints;
        /// <summary>
        /// Gets or sets the minimal pause between check pointing attempts.
        /// This setting defines how soon the checkpoint coordinator may trigger another checkpoint after it becomes possible to trigger another checkpoint with respect to the maximum number of concurrent checkpoints(<see cref="MaxConcurrentCheckpoints"/>).
        /// </summary>
        public long MinPauseBetweenCheckpoints
        {
            get => _minPauseBetweenCheckpoints;
            set => _minPauseBetweenCheckpoints = Assert.Positive(value);
        }

        private int _maxConcurrentCheckpoints = DefaultMaxConcurrentCheckpoints;
        /// <summary>
        /// Gets the maximum number of checkpoint attempts that may be in progress at the same time.
        /// </summary>
        public int MaxConcurrentCheckpoints
        {
            get => _maxConcurrentCheckpoints;
            set => _maxConcurrentCheckpoints = Assert.Positive(value);
        }

        /// <summary>
        /// Checks whether checkpointing is forced, despite currently non-checkpointable iteration feedback.
        /// </summary>
        public bool ForceCheckPointing { get; set; }
    }
}
