using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Common.Concurrent
{
    public class TimedCanceler : IAsyncCancellable
    {
        public TimedCanceler(
            TimeSpan dueTime,
            TimeSpan period,
            IRunnable callback)
        {
            DueTime = dueTime;
            Period = period;
            Timer = new Timer(delegate { callback.Run(); }, null, dueTime, period);
        }

        public TimeSpan DueTime { get; private set; }
        public TimeSpan Period { get; private set; }
        public Timer Timer { get; private set; }

        public void Cancel()
        {
            Timer.Change(Timeout.Infinite, Timeout.Infinite);
            Timer.Dispose();
            Timer = null;
        }

        public Task CancelAsync()
        {
            Cancel();

            return Task.CompletedTask;
        }
    }

    public class TimedCanceler<TState> : IAsyncCancellable<TState> where TState : class
    {
        public TimedCanceler(
            TimeSpan dueTime,
            TimeSpan period,
            IRunnable<TState> callback,
            TState state)
        {
            DueTime = dueTime;
            Period = period;
            Timer = new Timer(delegate (object obj) { callback.Run((TState)obj); }, state, dueTime, period);
        }

        public TimeSpan DueTime { get; private set; }
        public TimeSpan Period { get; private set; }
        public Timer Timer { get; private set; }
        public void Cancel(TState state)
        {
            Timer.Change(Timeout.Infinite, Timeout.Infinite);
            Timer.Dispose();
            Timer = null;
        }

        public Task CancelAsync(TState state)
        {
            Cancel(state);

            return Task.CompletedTask;
        }
    }
}
