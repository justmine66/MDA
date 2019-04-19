namespace MDA.Core.HeartBeat
{
    /// <summary>
    /// A heartbeat manager has to be able to start/stop monitoring a <see cref="IHeartbeatTarget{TPayLoad}"/>, and report heartbeat timeouts for this target.
    /// </summary>
    /// <typeparam name="I">Type of the incoming payload</typeparam>
    /// <typeparam name="O">Type of the outgoing payload</typeparam>
    public interface IHeartbeatManager<in I, out O> : IHeartbeatTarget<I>
    {
        /// <summary>
        /// Start monitoring a <see cref="IHeartbeatTarget{TPayLoad}"/>. Heartbeat timeouts for this target are reported to the <see cref="IHeartbeatListener{I,O}"/> associated with this heartbeat manager.
        /// </summary>
        /// <param name="resourceId">Resource ID identifying the heartbeat target.</param>
        /// <param name="heartbeatTarget">Interface to send heartbeat requests and responses to the heartbeat target</param>
        void MonitorTarget(string resourceId, IHeartbeatTarget<O> heartbeatTarget);

        /// <summary>
        /// Stops monitoring the heartbeat target with the associated resource ID.
        /// </summary>
        /// <param name="resourceId">Resource ID of the heartbeat target which shall no longer be monitored</param>
        void UnMonitorTarget(string resourceId);

        /// <summary>
        /// Stops the heartbeat manager.
        /// </summary>
        void Stop();

        /// <summary>
        /// Returns the last received heartbeat from the given target.
        /// </summary>
        /// <param name="resourceId">for which to return the last heartbeat.</param>
        /// <returns>Last heartbeat received from the given target or -1 if the target is not being monitored.</returns>
        long GetLastHeartbeatFrom(string resourceId);
    }
}
