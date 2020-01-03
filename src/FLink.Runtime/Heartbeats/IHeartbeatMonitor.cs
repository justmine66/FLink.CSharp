using FLink.Runtime.ClusterFramework.Types;
using FLink.Runtime.Concurrent;

namespace FLink.Runtime.Heartbeats
{
    /// <summary>
    /// Heartbeat monitor which manages the heartbeat state of the associated heartbeat target.
    /// The monitor notifies the <see cref="IHeartbeatListener{TInput,TOutput}"/> whenever it has not seen a heartbeat signal in the specified heartbeat timeout interval.
    /// Each heartbeat signal resets this timer.
    /// </summary>
    public interface IHeartbeatMonitor<in TPayload>
    {
        /// <summary>
        /// Gets heartbeat target.
        /// </summary>
        IHeartbeatTarget<TPayload> HeartbeatTarget { get; }

        /// <summary>
        /// Gets heartbeat target id.
        /// </summary>
        ResourceId HeartbeatTargetId { get; }

        /// <summary>
        /// Cancel this monitor.
        /// </summary>
        void Cancel();

        /// <summary>
        /// Gets the last heartbeat.
        /// </summary>
        long LastHeartbeat { get; }
    }

    /// <summary>
    /// This factory provides an indirection way to create <see cref="IHeartbeatMonitor{TPayload}"/>.
    /// </summary>
    /// <typeparam name="TPayload">Type of the outgoing heartbeat payload</typeparam>
    public interface IHeartbeatMonitorFactory<TPayload>
    {
        /// <summary>
        /// Create heartbeat monitor heartbeat monitor.
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="heartbeatTarget"></param>
        /// <param name="mainThreadExecutor"></param>
        /// <param name="heartbeatListener"></param>
        /// <param name="heartbeatTimeoutIntervalMs"></param>
        /// <returns></returns>
        IHeartbeatMonitor<TPayload> CreateHeartbeatMonitor(
            ResourceId resourceId,
            IHeartbeatTarget<TPayload> heartbeatTarget,
            IScheduledExecutor mainThreadExecutor,
            IHeartbeatListener<object, TPayload> heartbeatListener, 
            long heartbeatTimeoutIntervalMs);
    }
}
