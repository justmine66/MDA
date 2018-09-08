using System;

namespace MDA.Common.Scheduling
{
    /// <summary>
    /// Defines the system scheduler.
    /// </summary>
    public interface IScheduler
    {
        ICancellable Schedule(uint dueTimeInMilliseconds, uint periodInMilliseconds, Action action);
        ICancellable Schedule<TState>(uint dueTimeInMilliseconds, uint periodInMilliseconds, Action<TState> action, TState state);
        ICancellable ScheduleOnce(uint dueTimeInMilliseconds, Action action);
        ICancellable ScheduleOnce<TState>(uint dueTime, Action<TState> action, TState state);
    }
}
