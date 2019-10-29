namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// Configuration of state TTL(time-to-live) logic.
    /// </summary>
    public class StateTtlConfig
    {
        public UpdateType TtlUpdateType { get; protected set; }
        public StateVisibility TtlStateVisibility { get; protected set; }
        public TimeCharacteristic TtlTimeCharacteristic { get; protected set; }

        /// <summary>
        /// This option value configures when to update last access timestamp which prolongs state TTL.
        /// </summary>
        public enum UpdateType
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
        public enum StateVisibility
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
        public enum TimeCharacteristic
        {
            /// <summary>
            /// Processing time, see also <code>FLink.Streaming.Api.TimeCharacteristic.ProcessingTime</code>.
            /// </summary>
            ProcessingTime
        }
    }
}
