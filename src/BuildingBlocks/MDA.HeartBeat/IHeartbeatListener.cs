using System.Threading.Tasks;

namespace MDA.Core.HeartBeat
{
    /// <summary>
    /// Interface for the interaction with the <see cref="IHeartbeatManager{I,O}"/>. The heartbeat listener is used for the following things:
    /// 1. Notifications about heartbeat timeouts.
    /// 2. Payload reports of incoming heartbeats.
    /// 3. Retrieval of payloads for outgoing heartbeats.
    /// </summary>
    /// <typeparam name="I">Type of the incoming payload.</typeparam>
    /// <typeparam name="O">Type of the outgoing payload.</typeparam>
    public interface IHeartbeatListener<in I, O>
    {
        /// <summary>
        /// Callback which is called if a heartbeat for the machine identified by the given resource ID times out.
        /// </summary>
        /// <param name="resourceId">Resource ID of the machine whose heartbeat has timed out.</param>
        void NotifyHeartbeatTimeout(string resourceId);

        /// <summary>
        /// Callback which is called whenever a heartbeat with an associated payload is received. The carried payload is given to this method.
        /// </summary>
        /// <param name="resourceId">Resource ID identifying the sender of the payload.</param>
        /// <param name="payload">Payload of the received heartbeat.</param>
        void ReportPayload(string resourceId, I payload);

        /// <summary>
        /// Retrieves the payload value for the next heartbeat message. Since the operation can happen asynchronously, the result is returned wrapped in a future.
        /// </summary>
        /// <param name="resourceId">Resource ID identifying the receiver of the payload.</param>
        /// <returns>Future containing the next payload for heartbeats.</returns>
        TaskCompletionSource<O> RetrievePayload(string resourceId);
    }
}
