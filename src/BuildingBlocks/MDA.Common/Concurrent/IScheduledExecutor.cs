using System;
using System.Threading.Tasks;

namespace MDA.Common.Concurrent
{
    /// <summary>
    /// Provides a mechanism for executing a method at specified intervals.
    /// </summary>
    public interface IScheduledExecutor
    {
        /// <summary>
        /// Synchronously executes the given command after the given delay.
        /// </summary>
        /// <param name="command">the task to execute in the future.</param>
        /// <param name="delay">delay the time from now to delay the execution.</param>
        /// <returns>This never completes unless the given command execution failed or cancelled.</returns>
        ICancellable Schedule(IRunnable command, TimeSpan delay);

        /// <summary>
        /// Synchronously executes the given command after the given delay.
        /// </summary>
        /// <typeparam name="TState">the state type.</typeparam>
        /// <param name="command">the task to execute in the future.</param>
        /// <param name="delay">delay the time from now to delay the execution.</param>
        /// <param name="state">An object containing information to be used by the command.</param>
        /// <returns>This never completes unless the given command execution failed or cancelled.</returns>
        ICancellable<TState> Schedule<TState>(IRunnable<TState> command, TimeSpan delay, TState state) where TState : class;

        /// <summary>
        /// Asynchronously executes the given command after the given delay.
        /// </summary>
        /// <param name="command">the task to execute in the future.</param>
        /// <param name="delay">delay the time from now to delay the execution.</param>
        /// <returns>This never completes unless the given command execution failed or cancelled.</returns>
        Task<ICancellable> ScheduleAsync(IAsyncRunnable command, TimeSpan delay);

        /// <summary>
        /// Asynchronously executes the given command after the given delay.
        /// </summary>
        /// <typeparam name="TState">the state type.</typeparam>
        /// <param name="command">the task to execute in the future.</param>
        /// <param name="delay">delay the time from now to delay the execution.</param>
        /// <param name="state">An object containing information to be used by the command.</param>
        /// <returns>This never completes unless the given command execution failed or cancelled.</returns>
        Task<ICancellable<TState>> ScheduleAsync<TState>(IAsyncRunnable<TState> command, TimeSpan delay, TState state) where TState : class;

        /// <summary>
        /// Synchronously executes the given command periodically.
        /// </summary>
        /// <remarks>
        /// The first execution is started after the <see cref="initialDelay"/>, the second execution is started after the <see cref="initialDelay"/> + <see cref="period"/>, the third execution is started after the <see cref="initialDelay"/> + 2*<see cref="period"/> and so on.
        /// </remarks>
        /// <param name="command">the task to be executed periodically.</param>
        /// <param name="initialDelay">the time from now until the first execution is triggered.</param>
        /// <param name="period">the time after which the next execution is triggered.</param>
        /// <returns>This never completes unless the given command execution failed or cancelled.</returns>
        ICancellable ScheduleAtFixedRate(IRunnable command, TimeSpan initialDelay, TimeSpan period);

        /// <summary>
        /// Synchronously executes the given command periodically.
        /// </summary>
        /// <remarks>
        /// The first execution is started after the <see cref="initialDelay"/>, the second execution is started after the <see cref="initialDelay"/> + <see cref="period"/>, the third execution is started after the <see cref="initialDelay"/> + 2*<see cref="period"/> and so on.
        /// </remarks>
        /// <typeparam name="TState">the state type.</typeparam>
        /// <param name="command">the task to be executed periodically.</param>
        /// <param name="initialDelay">the time from now until the first execution is triggered.</param>
        /// <param name="period">the time after which the next execution is triggered.</param>
        /// <param name="state">An object containing information to be used by the command.</param>
        /// <returns>This never completes unless the given command execution failed or cancelled.</returns>
        ICancellable<TState> ScheduleAtFixedRate<TState>(IRunnable<TState> command, TimeSpan initialDelay, TimeSpan period, TState state) where TState : class;

        /// <summary>
        /// Asynchronously executes the given command periodically.
        /// </summary>
        /// <remarks>
        /// The first execution is started after the <see cref="initialDelay"/>, the second execution is started after the <see cref="initialDelay"/> + <see cref="period"/>, the third execution is started after the <see cref="initialDelay"/> + 2*<see cref="period"/> and so on.
        /// </remarks>
        /// <param name="command">the task to be executed periodically.</param>
        /// <param name="initialDelay">the time from now until the first execution is triggered.</param>
        /// <param name="period">the time after which the next execution is triggered.</param>
        /// <returns>This never completes unless the given command execution failed or cancelled.</returns>
        Task<ICancellable> ScheduleAtFixedRateAsync(IAsyncRunnable command, TimeSpan initialDelay, TimeSpan period);

        /// <summary>
        /// Asynchronously executes the given command periodically.
        /// </summary>
        /// <remarks>
        /// The first execution is started after the <see cref="initialDelay"/>, the second execution is started after the <see cref="initialDelay"/> + <see cref="period"/>, the third execution is started after the <see cref="initialDelay"/> + 2*<see cref="period"/> and so on.
        /// </remarks>
        /// <typeparam name="TState">the state type.</typeparam>
        /// <param name="command">the task to be executed periodically.</param>
        /// <param name="initialDelay">the time from now until the first execution is triggered.</param>
        /// <param name="period">the time after which the next execution is triggered.</param>
        /// <param name="state">An object containing information to be used by the command.</param>
        /// <returns>This never completes unless the given command execution failed or cancelled.</returns>
        Task<ICancellable<TState>> ScheduleAtFixedRateAsync<TState>(IAsyncRunnable<TState> command, TimeSpan initialDelay, TimeSpan period, TState state) where TState : class;

        /// <summary>
        /// Synchronously executes the given command repeatedly with the given delay between the end of an execution and the start of the next execution.
        /// </summary>
        /// <param name="command">the task to execute repeatedly.</param>
        /// <param name="initialDelay">the time from now until the first execution is triggered</param>
        /// <param name="delay">the time between the end of the current and the start of the next execution</param>
        /// <returns>This never completes unless the given command execution failed or cancelled.</returns>
        ICancellable ScheduleWithFixedDelay(IRunnable command, TimeSpan initialDelay, TimeSpan delay);

        /// <summary>
        /// Synchronously executes the given command repeatedly with the given delay between the end of an execution and the start of the next execution.
        /// </summary>
        /// <typeparam name="TState">the state type.</typeparam>
        /// <param name="command">the task to execute repeatedly.</param>
        /// <param name="initialDelay">the time from now until the first execution is triggered</param>
        /// <param name="delay">the time between the end of the current and the start of the next execution</param>
        /// <param name="state">An object containing information to be used by the command.</param>
        /// <returns>This never completes unless the given command execution failed or cancelled.</returns>
        ICancellable<TState> ScheduleWithFixedDelay<TState>(IRunnable<TState> command, TimeSpan initialDelay, TimeSpan delay, TState state) where TState : class;

        /// <summary>
        /// Asynchronously executes the given command repeatedly with the given delay between the end of an execution and the start of the next execution.
        /// </summary>
        /// <param name="command">the task to execute repeatedly.</param>
        /// <param name="initialDelay">the time from now until the first execution is triggered</param>
        /// <param name="delay">the time between the end of the current and the start of the next execution</param>
        /// <returns>This never completes unless the given command execution failed or cancelled.</returns>
        Task<ICancellable> ScheduleWithFixedDelayAsync(IAsyncRunnable command, TimeSpan initialDelay, TimeSpan delay);

        /// <summary>
        /// Asynchronously executes the given command repeatedly with the given delay between the end of an execution and the start of the next execution.
        /// </summary>
        /// <typeparam name="TState">the state type.</typeparam>
        /// <param name="command">the task to execute repeatedly.</param>
        /// <param name="initialDelay">the time from now until the first execution is triggered</param>
        /// <param name="delay">the time between the end of the current and the start of the next execution</param>
        /// <param name="state">An object containing information to be used by the command.</param>
        /// <returns>This never completes unless the given command execution failed or cancelled.</returns>
        Task<ICancellable<TState>> ScheduleWithFixedDelayAsync<TState>(IAsyncRunnable<TState> command, TimeSpan initialDelay, TimeSpan delay, TState state) where TState : class;
    }
}
