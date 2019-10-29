using System;
using System.Diagnostics.CodeAnalysis;
using FLink.Core.Util;

namespace FLink.Core.Api.Common.State
{
    using static Preconditions;

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

        private StateTtlConfig(
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
        public static Builder NewBuilder([NotNull]TimeSpan ttl) => new Builder(ttl);

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

        public class TtlCleanupStrategies
        {

        }

        public class Builder
        {
            public TimeSpan Ttl;
            public TtlUpdateType UpdateType;
            public TtlStateVisibility StateVisibility;
            public TtlTimeCharacteristic TimeCharacteristic;
            public TtlCleanupStrategies CleanupStrategies;

            public Builder([NotNull]TimeSpan ttl) => Ttl = ttl;

            public Builder SetUpdateType(TtlUpdateType updateType)
            {
                UpdateType = updateType;
                return this;
            }

            public Builder UpdateTtlOnCreateAndWrite() => SetUpdateType(TtlUpdateType.OnCreateAndWrite);

            public Builder UpdateTtlOnReadAndWrite() => SetUpdateType(TtlUpdateType.OnReadAndWrite);

            public Builder SetStateVisibility([NotNull]TtlStateVisibility stateVisibility)
            {
                StateVisibility = stateVisibility;
                return this;
            }

            public Builder ReturnExpiredIfNotCleanedUp() => SetStateVisibility(TtlStateVisibility.ReturnExpiredIfNotCleanedUp);

            public Builder NeverReturnExpired() => SetStateVisibility(TtlStateVisibility.NeverReturnExpired);

            public Builder SetTimeCharacteristic([NotNull]TtlTimeCharacteristic timeCharacteristic)
            {
                CheckArgument(timeCharacteristic.Equals(TtlTimeCharacteristic.ProcessingTime),
                    "Only support TimeCharacteristic.ProcessingTime, this function has replaced by setTtlTimeCharacteristic.");

                SetTtlTimeCharacteristic(TtlTimeCharacteristic.ProcessingTime);
                return this;
            }

            public Builder SetTtlTimeCharacteristic([NotNull]TtlTimeCharacteristic ttlTimeCharacteristic)
            {
                TimeCharacteristic = ttlTimeCharacteristic;
                return this;
            }

            public Builder UseProcessingTime() => SetTtlTimeCharacteristic(TtlTimeCharacteristic.ProcessingTime);
        }
    }
}
