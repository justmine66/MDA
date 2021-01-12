using MDA.Infrastructure.Concurrent;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Infrastructure.Scheduling
{
    /// <summary>
    /// Schedules <see cref="ITimerTask"/>s for one-time future execution in a background thread.
    /// </summary>
    public interface ITimer
    {
        /// <summary>
        /// Schedules the specified <see cref="ITimerTask"/> for one-time execution after the specified delay.
        /// </summary>
        /// <returns>a handle which is associated with the specified task</returns>
        /// <exception cref="InvalidOperationException">if this timer has been stopped already</exception>
        /// <exception cref="RejectedExecutionException">if the pending timeouts are too many and creating new timeout can cause instability in the system.</exception>
        ITimeout NewTimeout(ITimerTask task, TimeSpan delay);

        /// <summary>
        /// Releases all resources acquired by this <see cref="ITimer"/> and cancels all tasks which were scheduled but not executed yet.
        /// </summary>
        /// <returns>the handles associated with the tasks which were canceled by this method</returns>
        Task<ISet<ITimeout>> StopAsync(CancellationToken cancellationToken = default);
    }
}
