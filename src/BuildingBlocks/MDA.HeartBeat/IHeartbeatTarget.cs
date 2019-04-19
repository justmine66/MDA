namespace MDA.Core.HeartBeat
{
    /// <summary>
    /// Interface for components which can be sent heartbeats and from which one can request a heartbeat response.Both the heartbeat response as well as the heartbeat request can carry a payload.This payload is reported to the heartbeat target and contains additional information. The payload can be empty which is indicated by a null value.
    /// </summary>
    /// <typeparam name="TPayLoad">Type of the payload which is sent to the heartbeat target.</typeparam>
    public interface IHeartbeatTarget<in TPayLoad>
    {
        /// <summary>
        /// Sends a heartbeat response to the target. Each heartbeat response can carry a payload which contains additional information for the heartbeat target.
        /// </summary>
        /// <param name="heartbeatOrigin">Resource ID identifying the machine for which a heartbeat shall be reported.</param>
        /// <param name="heartbeatPayload">Payload of the heartbeat. Null indicates an empty payload.</param>
        void ReceiveHeartbeat(string heartbeatOrigin, TPayLoad heartbeatPayload);

        /// <summary>
        /// Requests a heartbeat from the target. Each heartbeat request can carry a payload which contains additional information for the heartbeat target.
        /// </summary>
        /// <param name="requestOrigin">Resource ID identifying the machine issuing the heartbeat request.</param>
        /// <param name="heartbeatPayload">Payload of the heartbeat request. Null indicates an empty payload.</param>
        void RequestHeartbeat(string requestOrigin, TPayLoad heartbeatPayload);
    }
}
