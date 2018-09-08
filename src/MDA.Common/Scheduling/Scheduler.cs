using System;

namespace MDA.Common.Scheduling
{
    /// <summary>
    /// Defines the system scheduler.
    /// </summary>
    public class Scheduler
    {
        public ICancellable Schedule(
            uint dueTimeInMilliseconds,
            uint periodInMilliseconds,
            Action action)
        {
            if (dueTimeInMilliseconds < 0)
            {
                throw new ArgumentException($"The {nameof(dueTimeInMilliseconds)} must be zero or greater.");
            }

            if (periodInMilliseconds < 1)
            {
                throw new ArgumentException($"The {nameof(periodInMilliseconds)} must be greater than zero.");
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return new TimedTask(dueTimeInMilliseconds, periodInMilliseconds, delegate (object state) { action(); });
        }

        public ICancellable Schedule<TState>(
            uint dueTimeInMilliseconds,
            uint periodInMilliseconds,
            Action<TState> action,
            TState state)
        {
            if (dueTimeInMilliseconds < 0)
            {
                throw new ArgumentException($"The {nameof(dueTimeInMilliseconds)} must be zero or greater.");
            }

            if (periodInMilliseconds < 1)
            {
                throw new ArgumentException($"The {nameof(periodInMilliseconds)} must be greater than zero.");
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            return new TimedTask(dueTimeInMilliseconds, periodInMilliseconds, delegate (object obj) { action(state); });
        }

        public ICancellable ScheduleOnce(uint dueTimeInMilliseconds, Action action)
        {
            if (dueTimeInMilliseconds < 0)
            {
                throw new ArgumentException($"The {nameof(dueTimeInMilliseconds)} must be zero or greater.");
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return new TimedTask(dueTimeInMilliseconds, 0, delegate (object obj) { action(); });
        }

        public ICancellable ScheduleOnce<TState>(uint dueTime, Action<TState> action, TState state)
        {
            if (dueTime < 0)
            {
                throw new ArgumentException($"The {nameof(dueTime)} must be zero or greater.");
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return new TimedTask(dueTime, 0, delegate (object obj) { action(state); });
        }
    }
}
