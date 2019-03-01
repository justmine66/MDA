using System.Threading;

namespace MDA.Common.Scheduling
{
    /// <summary>
    /// Defines an timed task.
    /// </summary>
    public class TimedTask : ICancellable
    {
        /// <summary>
        /// Initialize an <see cref="TimedTask"/> instance.
        /// </summary>
        /// <param name="dueTime">
        /// The amount of time to delay before callback is invoked, in milliseconds. Specify System.Threading.Timeout.Infinite to prevent the timer from starting. Specify zero (0) to start the timer
        /// immediately.
        /// </param>
        /// <param name="period">
        /// The time interval between invocations of callback, in milliseconds. Specify System.Threading.Timeout.Infinite to disable periodic signaling.
        /// </param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public TimedTask(
            uint dueTime,
            uint period,
            TimerCallback callback)
        {
            DueTime = dueTime;
            Period = period;
            Timer = new Timer(callback, null, dueTime, period);
        }

        public uint DueTime { get; private set; }
        public uint Period { get; private set; }
        public Timer Timer { get; private set; }

        public bool Cancelled => Timer == null;

        public void Cancel()
        {
            Timer.Change(Timeout.Infinite, Timeout.Infinite);
            Timer.Dispose();
            Timer = null;
        }
    }
}
