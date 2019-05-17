using System;
using System.Collections.Generic;
using System.Text;

namespace FLink.Core.Api.Common.State
{
    /// <summary>
    /// Configuration of state TTL(time-to-live) logic.
    /// </summary>
    public class StateTtlConfig
    {
        /// <summary>
        /// This option value configures when to update last access timestamp which prolongs state TTL.
        /// </summary>
        public enum UpdateType
        {
            /** TTL is disabled. State does not expire. */
            Disabled,
            /** Last access timestamp is initialized when state is created and updated on every write operation. */
            OnCreateAndWrite,
            /** The same as <code>OnCreateAndWrite</code> but also updated on read. */
            OnReadAndWrite
        }

        public UpdateType TtlUpdateType { get; protected set; }

        /// <summary>
        /// This option configures whether expired user value can be returned or not.
        /// </summary>
        public enum StateVisibility
        {
            /** Return expired user value if it is not cleaned up yet. */
            ReturnExpiredIfNotCleanedUp,
            /** Never return expired user value. */
            NeverReturnExpired
        }

        public StateVisibility TtlStateVisibility { get; protected set; }

        /// <summary>
        /// This option configures time scale to use for ttl.
        /// </summary>
        public enum TtlTimeCharacteristic
        {
            /** Processing time, see also <code>FLink.Streaming.Api.TimeCharacteristic.ProcessingTime</code>. */
            ProcessingTime
        }
    }
}
