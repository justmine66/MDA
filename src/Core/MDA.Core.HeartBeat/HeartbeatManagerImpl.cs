using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using MDA.Common;
using MDA.Common.Concurrent;
using MDA.Common.Scheduling;
using Microsoft.Extensions.Logging;

namespace MDA.Core.HeartBeat
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="O"></typeparam>
    public class HeartbeatManagerImpl<I, O> : IHeartbeatManager<I, O>
    {
        // Heartbeat timeout interval in millisecond.
        private readonly long _heartbeatTimeoutIntervalMs;
        // Resource ID which is used to mark one own's heartbeat signals.
        private readonly string _ownResourceId;
        // Heartbeat listener with which the heartbeat manager has been associated.
        private readonly IHeartbeatListener<I, O> _heartbeatListener;
        // Executor service used to run heartbeat timeout notifications.

        private readonly ILogger _logger;
        private readonly ConcurrentDictionary<string, HeartbeatManagerImpl.HeartbeatMonitor<O>> heartbeatTargets;

        public HeartbeatManagerImpl(ILogger logger)
        {
            _logger = logger;
        }

        public void ReceiveHeartbeat(string heartbeatOrigin, I heartbeatPayload)
        {
            throw new NotImplementedException();
        }

        public void RequestHeartbeat(string requestOrigin, I heartbeatPayload)
        {
            throw new NotImplementedException();
        }

        public void MonitorTarget(string resourceId, IHeartbeatTarget<O> heartbeatTarget)
        {
            throw new NotImplementedException();
        }

        public void UnMonitorTarget(string resourceId)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public long GetLastHeartbeatFrom(string resourceId)
        {
            throw new NotImplementedException();
        }

        private class HeartBeatMonitor<O> : IRunnable
        {
            // Resource ID of the monitored heartbeat target.
            private readonly string _resourceId;
            // Associated heartbeat target.
            private readonly IHeartbeatTarget<O> _heartBeatTarget;
            // Listener which is notified about heartbeat timeouts.
            private readonly IHeartbeatListener<object, O> _heartBeatListener;
            /** Executor service used to run heartbeat timeout notifications. */
            private readonly IScheduledExecutor _scheduledExecutor;
            // Maximum heartbeat timeout interval.
            private readonly long _heartBeatTimeoutIntervalMs;
            // Last heart beat
            private long _lastHeartBeat;

            private volatile TaskCompletionSource<object> _futureTimeout;

            private volatile State _state = State.Running;

            public HeartbeatMonitor(
                string resourceId,
                IHeartbeatTarget<O> heartbeatTarget,
                IScheduledExecutor scheduledExecutor,
                IHeartbeatListener<object, O> heartbeatListener,
                long heartbeatTimeoutIntervalMs)
            {
                Assert.Positive(heartbeatTimeoutIntervalMs, nameof(heartbeatTimeoutIntervalMs), "The heartbeat timeout interval has to be larger than 0.");

                _resourceId = resourceId;
                _heartBeatTarget = heartbeatTarget;
                _scheduledExecutor = scheduledExecutor;
                _heartBeatListener = heartbeatListener;
                _heartBeatTimeoutIntervalMs = heartbeatTimeoutIntervalMs;
                _lastHeartBeat = 0L;
            }

            private void ResetHeartbeatTimeout(long heartbeatTimeout)
            {
                
            }

            private enum State
            {
                Running,
                Timeout,
                Canceled
            }

            public void Run()
            {
                // The heartbeat has timed out if we're in state running
                if (Interlocked.CompareExchange(ref _state,))
                {
                    heartbeatListener.notifyHeartbeatTimeout(resourceID);
                }
            }
        }
    }
}
