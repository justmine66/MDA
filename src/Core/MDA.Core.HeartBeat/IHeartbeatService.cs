namespace MDA.Core.HeartBeat
{
    /// <summary>
    /// HeartbeatService gives access to all services needed for heart beating. This includes the creation of heartbeat receivers and heartbeat senders.
    /// </summary>
    public interface IHeartbeatService
    {
        /// <summary>
        /// Heartbeat interval for the created services.
        /// </summary>
        long HeartBeatInterval { get; }

        /// <summary>
        /// Heartbeat timeout for the created services.
        /// </summary>
        long HeartBeatTimeout { get; }

        IHeartbeatManager<I, O> CreateHeartbeatManager<I, O>(
            string resourceId, 
            IHeartbeatListener<I, O> heartbeatListener,);
    }
}
