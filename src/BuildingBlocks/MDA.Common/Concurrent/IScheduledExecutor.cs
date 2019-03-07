using System;
using System.Threading.Tasks;

namespace MDA.Common.Concurrent
{
    public interface IScheduledExecutor
    {
        /// <summary>
        /// Executes the given command after the given delay.
        /// </summary>
        /// <param name="command">the task to execute in the future.</param>
        /// <param name="delay">delay the time from now to delay the execution.</param>
        /// <returns></returns>
        Task ScheduleAsync(IRunnable command, TimeSpan delay);

        /// <summary>
        /// Executes the given command periodically.
        /// </summary>
        /// <remarks>
        /// The first execution is started after the <see cref="initialDelay"/>, the second execution is started after the <see cref="initialDelay"/> + <see cref="period"/>, the third execution is started after the <see cref="initialDelay"/> + 2*<see cref="period"/> and so on.
        /// </remarks>
        /// <param name="command">the task to be executed periodically.</param>
        /// <param name="initialDelay">the time from now until the first execution is triggered.</param>
        /// <param name="period">the time after which the next execution is triggered.</param>
        /// <returns>a ScheduledFuture representing the periodic task. This future never completes unless an execution of the given task fails or if the future is cancelled.</returns>
        TaskCompletionSource<object> ScheduleAtFixedRateAsync(IRunnable command, TimeSpan initialDelay, TimeSpan period);

        /// <summary>
        /// Executes the given command periodically.
        /// </summary>
        /// <remarks>
        /// The first execution is started after the <see cref="initialDelay"/>, the second execution is started after the <see cref="initialDelay"/> + <see cref="period"/>, the third execution is started after the <see cref="initialDelay"/> + 2*<see cref="period"/> and so on.
        /// </remarks>
        /// <typeparam name="TState">The state</typeparam>
        /// <param name="command">the task to be executed periodically.</param>
        /// <param name="initialDelay">the time from now until the first execution is triggered.</param>
        /// <param name="period">the time after which the next execution is triggered.</param>
        /// <returns>a ScheduledFuture representing the periodic task. This future never completes unless an execution of the given task fails or if the future is cancelled.</returns>
        TaskCompletionSource<TState> ScheduleAtFixedRateAsync<TState>(IRunnable<TState> command, TimeSpan initialDelay, TimeSpan period);

        /// <summary>
        /// Executed the given command repeatedly with the given delay between the end of an execution and the start of the next execution.
        /// </summary>
        /// <typeparam name="T">The state</typeparam>
        /// <param name="command">the task to execute repeatedly.</param>
        /// <param name="initialDelay">the time from now until the first execution is triggered</param>
        /// <param name="delay">the time between the end of the current and the start of the next execution</param>
        /// <returns>a ScheduledFuture representing the repeatedly executed task. This future never completes unless the execution of the given task fails or if the future is cancelled.</returns>
        TaskCompletionSource<object> ScheduleWithFixedDelayAsync(IRunnable command, TimeSpan initialDelay, TimeSpan delay);

        /// <summary>
        /// Executed the given command repeatedly with the given delay between the end of an execution and the start of the next execution.
        /// </summary>
        /// <typeparam name="TState">The state</typeparam>
        /// <param name="command">the task to execute repeatedly.</param>
        /// <param name="initialDelay">the time from now until the first execution is triggered</param>
        /// <param name="delay">the time between the end of the current and the start of the next execution</param>
        /// <returns>a ScheduledFuture representing the repeatedly executed task. This future never completes unless the execution of the given task fails or if the future is cancelled.</returns>
        TaskCompletionSource<TState> ScheduleWithFixedDelayAsync<TState>(IRunnable<TState> command, TimeSpan initialDelay, TimeSpan delay);
    }
}
