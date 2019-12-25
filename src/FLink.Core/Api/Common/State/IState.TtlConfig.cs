using System;
using System.Collections.Generic;
using FLink.Core.Util;

namespace FLink.Core.Api.Common.State
{
    using static Preconditions;

    /// <summary>
    /// This option value configures when to update last access timestamp which prolongs state TTL.
    /// </summary>
    public enum TtlStateUpdateType
    {
        /// <summary>
        /// TTL is disabled. State does not expire.
        /// </summary>
        Disabled,
        /// <summary>
        /// Last access timestamp is initialized when state is created and updated on every write operation. 
        /// </summary>
        OnCreateAndWrite,
        /// <summary>
        /// The same as <code>OnCreateAndWrite</code> but also updated on read.
        /// </summary>
        OnReadAndWrite
    }

    /// <summary>
    /// This option configures whether expired user value can be returned or not.
    /// </summary>
    public enum TtlStateVisibility
    {
        /// <summary>
        /// Return expired user value if it is not cleaned up yet.
        /// </summary>
        ReturnExpiredIfNotCleanedUp,
        /// <summary>
        /// Never return expired user value.
        /// </summary>
        NeverReturnExpired
    }

    /// <summary>
    /// This option configures time scale to use for ttl.
    /// </summary>
    public enum TtlTimeCharacteristic
    {
        /// <summary>
        /// Processing time, see also <code>FLink.Streaming.Api.TimeCharacteristic.ProcessingTime</code>.
        /// </summary>
        ProcessingTime
    }

    /// <summary>
    /// TTL cleanup strategies.
    /// This class configures when to cleanup expired state with TTL.
    /// By default, state is always cleaned up on explicit read access if found expired.
    /// Currently cleanup of state full snapshot can be additionally activated.
    /// </summary>
    public class TtlCleanupStrategies
    {
        /// <summary>
        /// Base interface for cleanup strategies configurations.
        /// </summary>
        public interface ICleanupStrategy { }

        public enum Strategies
        {
            FullStateScanSnapshot,
            IncrementalCleanup,
            RocksDbCompactionFilter
        }

        public Dictionary<Strategies, ICleanupStrategy> CleanupStrategies;
        public bool IsCleanupInBackground;

        public TtlCleanupStrategies(Dictionary<Strategies, ICleanupStrategy> strategies, bool isCleanupInBackground)
        {
            CleanupStrategies = strategies;
            IsCleanupInBackground = isCleanupInBackground;
        }
    }

    /// <summary>
    /// Configuration of state TTL(time-to-live) logic.
    /// </summary>
    public class StateTtlConfig
    {
        public static readonly StateTtlConfig Disabled = new StateTtlConfigBuilder(TimeSpan.FromSeconds(long.MaxValue))
            .SetUpdateType(TtlStateUpdateType.Disabled)
            .Build();

        public TimeSpan Ttl { get; }
        public TtlStateUpdateType UpdateType { get; }
        public TtlStateVisibility StateVisibility { get; }
        public TtlTimeCharacteristic TimeCharacteristic { get; }
        public TtlCleanupStrategies CleanupStrategies { get; }

        public bool IsEnabled => UpdateType != TtlStateUpdateType.Disabled;

        public StateTtlConfig(
            TtlStateUpdateType updateType,
            TtlStateVisibility stateVisibility,
            TtlTimeCharacteristic ttlTimeCharacteristic,
            TimeSpan ttl,
            TtlCleanupStrategies cleanupStrategies)
        {
            UpdateType = CheckNotNull(updateType);
            StateVisibility = CheckNotNull(stateVisibility);
            TimeCharacteristic = CheckNotNull(ttlTimeCharacteristic);
            Ttl = CheckNotNull(ttl);
            CleanupStrategies = cleanupStrategies;

            CheckArgument(ttl.TotalMilliseconds > 0, "TTL is expected to be positive.");
        }

        public override string ToString() =>
            $"StateTtlConfig{{{nameof(UpdateType)}={UpdateType}, {nameof(StateVisibility)}={StateVisibility}, {nameof(TimeCharacteristic)}={TimeCharacteristic}, {nameof(Ttl)}={Ttl}{'}'}";

        public static StateTtlConfigBuilder NewBuilder(TimeSpan ttl) => new StateTtlConfigBuilder(ttl);
    }

    public class StateTtlConfigBuilder
    {
        private readonly TimeSpan _ttl;
        private TtlStateUpdateType _updateType;
        private TtlStateVisibility _stateVisibility;
        private TtlTimeCharacteristic _timeCharacteristic;
        private readonly Dictionary<TtlCleanupStrategies.Strategies, TtlCleanupStrategies.ICleanupStrategy> _cleanupStrategies = new Dictionary<TtlCleanupStrategies.Strategies, TtlCleanupStrategies.ICleanupStrategy>();
        private bool _isCleanupInBackground = true;

        public StateTtlConfigBuilder(TimeSpan ttl) => _ttl = ttl;

        public StateTtlConfigBuilder SetUpdateType(TtlStateUpdateType updateType)
        {
            _updateType = updateType;
            return this;
        }

        public StateTtlConfigBuilder UpdateTtlOnCreateAndWrite() => SetUpdateType(TtlStateUpdateType.OnCreateAndWrite);

        public StateTtlConfigBuilder UpdateTtlOnReadAndWrite() => SetUpdateType(TtlStateUpdateType.OnReadAndWrite);

        public StateTtlConfigBuilder SetStateVisibility(TtlStateVisibility stateVisibility)
        {
            _stateVisibility = stateVisibility;
            return this;
        }

        public StateTtlConfigBuilder ReturnExpiredIfNotCleanedUp() => SetStateVisibility(TtlStateVisibility.ReturnExpiredIfNotCleanedUp);

        public StateTtlConfigBuilder NeverReturnExpired() => SetStateVisibility(TtlStateVisibility.NeverReturnExpired);

        public StateTtlConfigBuilder SetTimeCharacteristic(TtlTimeCharacteristic timeCharacteristic)
        {
            CheckArgument(timeCharacteristic.Equals(TtlTimeCharacteristic.ProcessingTime),
                "Only support TimeCharacteristic.ProcessingTime, this function has replaced by setTtlTimeCharacteristic.");

            SetTtlTimeCharacteristic(TtlTimeCharacteristic.ProcessingTime);
            return this;
        }

        public StateTtlConfigBuilder SetTtlTimeCharacteristic(TtlTimeCharacteristic ttlTimeCharacteristic)
        {
            _timeCharacteristic = ttlTimeCharacteristic;
            return this;
        }

        public StateTtlConfigBuilder UseProcessingTime() => SetTtlTimeCharacteristic(TtlTimeCharacteristic.ProcessingTime);

        public StateTtlConfig Build()
        {
            return new StateTtlConfig(
                _updateType,
                _stateVisibility,
                _timeCharacteristic,
                _ttl,
                new TtlCleanupStrategies(_cleanupStrategies, _isCleanupInBackground));
        }
    }
}
