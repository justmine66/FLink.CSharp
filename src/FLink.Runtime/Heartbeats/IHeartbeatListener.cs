using FLink.Runtime.ClusterFramework.Types;

namespace FLink.Runtime.Heartbeats
{
    /// <summary>
    /// Interface for the interaction with the <see cref="IHeartbeatManager{TInput,TOutput}"/>.
    /// The heartbeat listener is used for the following things:
    /// 1. Notifications about heartbeat timeouts
    /// 2. Payload reports of incoming heartbeats
    /// 3. Retrieval of payloads for outgoing heartbeats
    /// </summary>
    /// <typeparam name="TInput">Type of the incoming payload</typeparam>
    /// <typeparam name="TOutput">Type of the outgoing payload</typeparam>
    public interface IHeartbeatListener<in TInput, out TOutput>
    {
        /// <summary>
        /// Callback which is called if a heartbeat for the machine identified by the given resource ID times out.
        /// </summary>
        /// <param name="resourceId"></param>
        void NotifyHeartbeatTimeout(ResourceId resourceId);

        /// <summary>
        /// Callback which is called whenever a heartbeat with an associated payload is received. The carried payload is given to this method.
        /// </summary>
        /// <param name="resourceId">Resource Id identifying the sender of the payload</param>
        /// <param name="payload">Payload of the received heartbeat</param>
        void ReportPayload(ResourceId resourceId, TInput payload);

        /// <summary>
        /// Retrieves the payload value for the next heartbeat message.
        /// </summary>
        /// <param name="resourceId">Resource ID identifying the receiver of the payload</param>
        /// <returns>The payload for the next heartbeat</returns>
        TOutput RetrievePayload(ResourceId resourceId);
    }
}
