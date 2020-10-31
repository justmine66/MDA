using MDA.Infrastructure.Utils;
using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Infrastructure.Async
{
    /// <summary>
    /// This class is a convenient utility class to execute a certain asynchronous function with retries,
    /// allowing to specify custom retry filters and policies.
    /// </summary>
    public static class AsyncExecutorWithRetries
    {
        public const int InfiniteRetries = Timeout.Infinite;
        public static readonly TimeSpan InfiniteTimespan = Timeout.InfiniteTimeSpan;
        public static readonly IBackOffProvider DefaultBackOffProvider = new FixedStepExponentialBackOff(TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(1));

        /// <summary>
        /// Execute a given function a number of times, based on retry configuration parameters.
        /// </summary>
        public static Task ExecuteWithRetriesAsync(
            Func<int, Task> userDefinedFunction,
            int maxNumErrorTries,
            Func<Exception, int, bool> retryExceptionFilter,
            TimeSpan maxExecutionTime,
            IBackOffProvider onErrorBackOff = null)
        {
            async Task<bool> WrapFunction(int i)
            {
                await userDefinedFunction(i);

                return true;
            }

            return DoExecuteWithRetriesAsync(
                WrapFunction,
                0,
                0,
                maxNumErrorTries,
                maxExecutionTime,
                DateTime.UtcNow,
                null,
                retryExceptionFilter,
                null,
                onErrorBackOff);
        }

        /// <summary>
        /// Execute a given function a number of times, based on retry configuration parameters.
        /// </summary>
        public static Task<T> ExecuteWithRetriesAsync<T>(
            Func<int, Task<T>> userDefinedFunction,
            int maxNumErrorTries,
            Func<Exception, int, bool> retryExceptionFilter,
            TimeSpan maxExecutionTime,
            IBackOffProvider onErrorBackOff = null)
        {
            return ExecuteWithRetriesAsync<T>(
                userDefinedFunction,
                0,
                maxNumErrorTries,
                null,
                retryExceptionFilter,
                maxExecutionTime,
                null,
                onErrorBackOff);
        }

        /// <summary>
        /// Execute a given function a number of times, based on retry configuration parameters.
        /// </summary>
        /// <param name="userDefinedFunction">Function to execute</param>
        /// <param name="maxNumSuccessTries">Maximal number of successful execution attempts.
        /// ExecuteWithRetries will try to re-execute the given function again if directed so by retryValueFilter.
        /// Set to -1 for unlimited number of success retries, until retryValueFilter is satisfied.
        /// Set to 0 for only one success attempt, which will cause retryValueFilter to be ignored and the given function executed only once until first success.</param>
        /// <param name="maxNumErrorTries">Maximal number of execution attempts due to errors.
        /// Set to -1 for unlimited number of error retries, until retryExceptionFilter is satisfied.</param>
        /// <param name="retryValueFilter">Filter function to indicate if successful execution should be retied.
        /// Set to null to disable successful retries.</param>
        /// <param name="retryExceptionFilter">Filter function to indicate if error execution should be retied.
        /// Set to null to disable error retries.</param>
        /// <param name="maxExecutionTime">The maximal execution time of the ExecuteWithRetries function.</param>
        /// <param name="onSuccessBackOff">The back off provider object, which determines how much to wait between success retries.</param>
        /// <param name="onErrorBackOff">The back off provider object, which determines how much to wait between error retries.</param>
        /// <returns></returns>
        public static Task<T> ExecuteWithRetriesAsync<T>(
            Func<int, Task<T>> userDefinedFunction,
            int maxNumSuccessTries,
            int maxNumErrorTries,
            Func<T, int, bool> retryValueFilter,
            Func<Exception, int, bool> retryExceptionFilter,
            TimeSpan maxExecutionTime = default,
            IBackOffProvider onSuccessBackOff = null,
            IBackOffProvider onErrorBackOff = null)
        {
            return DoExecuteWithRetriesAsync<T>(
                userDefinedFunction,
                0,
                maxNumSuccessTries,
                maxNumErrorTries,
                maxExecutionTime,
                DateTime.UtcNow,
                retryValueFilter,
                retryExceptionFilter,
                onSuccessBackOff,
                onErrorBackOff);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static async Task<T> DoExecuteWithRetriesAsync<T>(
            Func<int, Task<T>> userDefinedFunction,
            int callCounter,
            int maxNumSuccessTries,
            int maxNumErrorTries,
            TimeSpan maxExecutionTime,
            DateTime startExecutionTime,
            Func<T, int, bool> retryValueFilter = null,
            Func<Exception, int, bool> retryExceptionFilter = null,
            IBackOffProvider onSuccessBackOff = null,
            IBackOffProvider onErrorBackOff = null)
        {
            onSuccessBackOff ??= DefaultBackOffProvider;
            onErrorBackOff ??= DefaultBackOffProvider;

            var result = default(T);
            ExceptionDispatchInfo lastExceptionInfo = null;
            bool needRetry;

            do
            {
                needRetry = false;

                if (maxExecutionTime != InfiniteTimespan &&
                    maxExecutionTime != default)
                {
                    var now = DateTime.UtcNow;
                    if (now - startExecutionTime > maxExecutionTime)
                    {
                        if (lastExceptionInfo == null)
                        {
                            throw new TimeoutException(
                                $"ExecuteWithRetries has exceeded its max execution time of {maxExecutionTime}. Now is {LogFormatter.PrintDate(now)}, started at {LogFormatter.PrintDate(startExecutionTime)}, passed {now - startExecutionTime}");
                        }

                        lastExceptionInfo.Throw();
                    }
                }

                var counter = callCounter;

                try
                {
                    callCounter++;
                    result = await userDefinedFunction(counter);
                    lastExceptionInfo = null;

                    if (callCounter < maxNumSuccessTries ||
                        maxNumSuccessTries == InfiniteRetries) // -1 for infinite retries
                    {
                        if (retryValueFilter != null)
                            needRetry = retryValueFilter(result, counter);
                    }

                    if (needRetry)
                    {
                        var delay = onSuccessBackOff?.Next(counter);

                        if (delay.HasValue)
                        {
                            await Task.Delay(delay.Value);
                        }
                    }
                }
                catch (Exception exc)
                {
                    needRetry = false;

                    if (callCounter < maxNumErrorTries ||
                        maxNumErrorTries == InfiniteRetries)
                    {
                        if (retryExceptionFilter != null)
                            needRetry = retryExceptionFilter(exc, counter);
                    }

                    if (!needRetry)
                    {
                        throw;
                    }

                    lastExceptionInfo = ExceptionDispatchInfo.Capture(exc);

                    var delay = onErrorBackOff?.Next(counter);

                    if (delay.HasValue)
                    {
                        await Task.Delay(delay.Value);
                    }
                }
            } while (needRetry);

            return result;
        }
    }

    // Allow multiple implementations of the backOff algorithm.
    // For instance, ConstantBackOff variation that always waits for a fixed timespan,
    // or a RateLimitingBackOff that keeps makes sure that some minimum time period occurs between calls to some API
    // (especially useful if you use the same instance for multiple potentially simultaneous calls to ExecuteWithRetries).
    // Implementations should be immutable.
    // If mutable state is needed, extend the next function to pass the state from the caller.
    // example: TimeSpan Next(int attempt, object state, out object newState);
    public interface IBackOffProvider
    {
        TimeSpan Next(int attempt);
    }

    /// <summary>
    /// 固定回退算法
    /// </summary>
    public class FixedBackOff : IBackOffProvider
    {
        private readonly TimeSpan _fixedDelay;

        public FixedBackOff(TimeSpan delay)
        {
            _fixedDelay = delay;
        }

        public TimeSpan Next(int attempt)
        {
            return _fixedDelay;
        }
    }

    /// <summary>
    /// 随机步长指数退避算法
    /// 每次通过指数计算出延迟，然后在区间[最小延迟,当前延迟]取随机值.
    /// 示例：1，3，2，9
    /// </summary>
    public class RandomStepExponentialBackOff : IBackOffProvider
    {
        private readonly TimeSpan _minDelay;
        private readonly TimeSpan _maxDelay;
        private readonly TimeSpan _step;
        private readonly SafeRandom _random;

        public RandomStepExponentialBackOff(TimeSpan minDelay, TimeSpan maxDelay, TimeSpan step)
        {
            if (minDelay <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(minDelay), minDelay, "ExponentialBackOff min delay must be a positive number.");
            if (maxDelay <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(maxDelay), maxDelay, "ExponentialBackOff max delay must be a positive number.");
            if (step <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(step), step, "ExponentialBackOff step must be a positive number.");
            if (minDelay >= maxDelay)
                throw new ArgumentOutOfRangeException(nameof(minDelay), minDelay, "ExponentialBackOff min delay must be less than max delay.");

            _minDelay = minDelay;
            _maxDelay = maxDelay;
            _step = step;
            _random = new SafeRandom();
        }

        public TimeSpan Next(int attempt)
        {
            TimeSpan currentDelay;

            try
            {
                long multiple = checked(1 << attempt);

                currentDelay = _step.Multiply(multiple);

                if (currentDelay <= TimeSpan.Zero)
                    throw new OverflowException();
            }
            catch (OverflowException)
            {
                currentDelay = _maxDelay;
            }

            currentDelay = TimeSpanExtensions.Min(currentDelay, _maxDelay);

            if (_minDelay >= currentDelay)
                throw new ArgumentOutOfRangeException($"minDelay {_minDelay}, currentDelay = {currentDelay}");

            return _random.NextTimeSpan(_minDelay, currentDelay);
        }
    }

    /// <summary>
    /// 固定步长指数退避算法
    /// 示例：1，2，4，8
    /// </summary>
    public class FixedStepExponentialBackOff : IBackOffProvider
    {
        private readonly TimeSpan _minDelay;
        private readonly TimeSpan _maxDelay;
        private readonly TimeSpan _step;

        public FixedStepExponentialBackOff(TimeSpan minDelay, TimeSpan maxDelay, TimeSpan step)
        {
            if (minDelay <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(minDelay), minDelay, "FixedStepExponentialBackOff min delay must be a positive number.");
            if (maxDelay <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(maxDelay), maxDelay, "FixedStepExponentialBackOff max delay must be a positive number.");
            if (step <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(step), step, "FixedStepExponentialBackOff step must be a positive number.");
            if (minDelay >= maxDelay)
                throw new ArgumentOutOfRangeException(nameof(minDelay), minDelay, "FixedStepExponentialBackOff min delay must be less than max delay.");

            _minDelay = minDelay;
            _maxDelay = maxDelay;
            _step = step;
        }

        public TimeSpan Next(int attempt)
        {
            TimeSpan currentDelay;

            try
            {
                long multiple = checked(1 << attempt);

                currentDelay = _step.Multiply(multiple);

                if (currentDelay <= TimeSpan.Zero)
                    throw new OverflowException();
            }
            catch (OverflowException)
            {
                currentDelay = _maxDelay;
            }

            currentDelay = TimeSpanExtensions.Min(currentDelay, _maxDelay);

            return TimeSpanExtensions.Max(_minDelay, currentDelay);
        }
    }
}
