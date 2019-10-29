using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FLink.Core.Util;

namespace FLink.Core.Api.Common.State
{
    using static Preconditions;

    /// <summary>
    /// This option value configures when to update last access timestamp which prolongs state TTL.
    /// </summary>
    public enum TtlUpdateType
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
        public TimeSpan Ttl;
        public TtlUpdateType UpdateType;
        public TtlStateVisibility StateVisibility;
        public TtlTimeCharacteristic TimeCharacteristic;
        public TtlCleanupStrategies CleanupStrategies;

        public bool IsEnabled => UpdateType != TtlUpdateType.Disabled;

        public StateTtlConfig(
            TtlUpdateType updateType,
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

        public override string ToString() => "StateTtlConfig{" +
                                             "updateType=" + UpdateType +
                                             ", stateVisibility=" + StateVisibility +
                                             ", ttlTimeCharacteristic=" + TimeCharacteristic +
                                             ", ttl=" + Ttl +
                                             '}';
        public static StateTtlConfigBuilder NewBuilder([NotNull]TimeSpan ttl) => new StateTtlConfigBuilder(ttl);
    }

    public class StateTtlConfigBuilder
    {
        public TimeSpan Ttl;
        public TtlUpdateType UpdateType;
        public TtlStateVisibility StateVisibility;
        public TtlTimeCharacteristic TimeCharacteristic;
        public Dictionary<TtlCleanupStrategies.Strategies, TtlCleanupStrategies.ICleanupStrategy> CleanupStrategies;
        public bool IsCleanupInBackground;

        public StateTtlConfigBuilder([NotNull]TimeSpan ttl) => Ttl = ttl;

        public StateTtlConfigBuilder SetUpdateType(TtlUpdateType updateType)
        {
            UpdateType = updateType;
            return this;
        }

        public StateTtlConfigBuilder UpdateTtlOnCreateAndWrite() => SetUpdateType(TtlUpdateType.OnCreateAndWrite);

        public StateTtlConfigBuilder UpdateTtlOnReadAndWrite() => SetUpdateType(TtlUpdateType.OnReadAndWrite);

        public StateTtlConfigBuilder SetStateVisibility([NotNull]TtlStateVisibility stateVisibility)
        {
            StateVisibility = stateVisibility;
            return this;
        }

        public StateTtlConfigBuilder ReturnExpiredIfNotCleanedUp() => SetStateVisibility(TtlStateVisibility.ReturnExpiredIfNotCleanedUp);

        public StateTtlConfigBuilder NeverReturnExpired() => SetStateVisibility(TtlStateVisibility.NeverReturnExpired);

        public StateTtlConfigBuilder SetTimeCharacteristic([NotNull]TtlTimeCharacteristic timeCharacteristic)
        {
            CheckArgument(timeCharacteristic.Equals(TtlTimeCharacteristic.ProcessingTime),
                "Only support TimeCharacteristic.ProcessingTime, this function has replaced by setTtlTimeCharacteristic.");

            SetTtlTimeCharacteristic(TtlTimeCharacteristic.ProcessingTime);
            return this;
        }

        public StateTtlConfigBuilder SetTtlTimeCharacteristic([NotNull]TtlTimeCharacteristic ttlTimeCharacteristic)
        {
            TimeCharacteristic = ttlTimeCharacteristic;
            return this;
        }

        public StateTtlConfigBuilder UseProcessingTime() => SetTtlTimeCharacteristic(TtlTimeCharacteristic.ProcessingTime);

        public StateTtlConfig Build()
        {
            return new StateTtlConfig(
                UpdateType,
                StateVisibility,
                TimeCharacteristic,
                Ttl,
                new TtlCleanupStrategies(CleanupStrategies, IsCleanupInBackground));
        }
    }
}
