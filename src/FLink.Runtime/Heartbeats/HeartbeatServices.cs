using FLink.Core.Configurations;
using FLink.Core.Util;
using FLink.Runtime.ClusterFramework.Types;
using FLink.Runtime.Concurrent;
using Microsoft.Extensions.Logging;

namespace FLink.Runtime.Heartbeats
{
    /// <summary>
    /// HeartbeatServices gives access to all services needed for heartbeating.
    /// This includes the creation of heartbeat receivers and heartbeat senders.
    /// </summary>
    public class HeartbeatServices
    {
        public HeartbeatServices(long heartbeatInterval, long heartbeatTimeout)
        {
            Preconditions.CheckArgument(0L < heartbeatInterval, "The heartbeat interval must be larger than 0.");
            Preconditions.CheckArgument(heartbeatInterval <= heartbeatTimeout, "The heartbeat timeout should be larger or equal than the heartbeat interval.");

            HeartbeatInterval = heartbeatInterval;
            HeartbeatTimeout = heartbeatTimeout;
        }

        /// <summary>
        /// Heartbeat interval for the created services.
        /// </summary>
        public long HeartbeatInterval { get; }

        /// <summary>
        /// Heartbeat timeout for the created services.
        /// </summary>
        public long HeartbeatTimeout { get; }

        /// <summary>
        /// Creates a heartbeat manager which does not actively send heartbeats.
        /// </summary>
        /// <typeparam name="TInput">Type of the incoming payload</typeparam>
        /// <typeparam name="TOutput">Type of the outgoing payload</typeparam>
        /// <param name="resourceId">which identifies the owner of the heartbeat manager</param>
        /// <param name="heartbeatListener">which will be notified upon heartbeat timeouts for registered targets</param>
        /// <param name="mainThreadExecutor">Scheduled executor to be used for scheduling heartbeat timeouts</param>
        /// <param name="log">to be used for the logging</param>
        /// <returns>A new HeartbeatManager instance</returns>
        public IHeartbeatManager<TInput, TOutput> CreateHeartbeatManager<TInput, TOutput>(
            ResourceId resourceId,
            IHeartbeatListener<TInput, TOutput> heartbeatListener,
            IScheduledExecutor mainThreadExecutor,
            ILogger log)
        {
            return null;
        }

        /// <summary>
        /// Creates a heartbeat manager which actively sends heartbeats to monitoring targets.
        /// </summary>
        /// <typeparam name="TInput">Type of the incoming payload</typeparam>
        /// <typeparam name="TOutput">Type of the outgoing payload</typeparam>
        /// <param name="resourceId">Resource Id which identifies the owner of the heartbeat manager</param>
        /// <param name="heartbeatListener">Listener which will be notified upon heartbeat timeouts for registered targets</param>
        /// <param name="mainThreadExecutor">Scheduled executor to be used for scheduling heartbeat timeouts and periodically send heartbeat requests</param>
        /// <param name="log">Logger to be used for the logging</param>
        /// <returns>A new HeartbeatManager instance which actively sends heartbeats</returns>
        public IHeartbeatManager<TInput, TOutput> CreateHeartbeatManagerSender<TInput, TOutput>(
            ResourceId resourceId,
            IHeartbeatListener<TInput, TOutput> heartbeatListener,
            IScheduledExecutor mainThreadExecutor,
            ILogger log)
        {
            return null;
        }

        public static HeartbeatServices FromConfiguration(Configuration configuration)
        {
            var heartbeatInterval = configuration.GetLong(HeartbeatManagerOptions.HeartbeatInterval);
            var heartbeatTimeout = configuration.GetLong(HeartbeatManagerOptions.HeartbeatTimeout);

            return new HeartbeatServices(heartbeatInterval, heartbeatTimeout);
        }
    }
}
