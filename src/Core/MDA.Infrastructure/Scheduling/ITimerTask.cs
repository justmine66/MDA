namespace MDA.Infrastructure.Scheduling
{
    /// <summary>
    /// A task which is executed after the delay specified with
    /// <see cref="ITimer.NewTimeout"/>.
    /// </summary>
    public interface ITimerTask
    {
        /// <summary>
        /// Executed after the delay specified with
        /// <see cref="ITimer.NewTimeout"/>.
        /// </summary>
        /// <param name="timeout">a handle which is associated with this task</param>
        void Run(ITimeout timeout);
    }
}
