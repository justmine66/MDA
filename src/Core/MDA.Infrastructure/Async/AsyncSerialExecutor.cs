using MDA.Infrastructure.Concurrent;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace MDA.Infrastructure.Async
{
    /// <summary>
    /// A utility class that provides serial execution of async functions.
    /// In can be used inside re-entrant code to execute some methods in a non-re-entrant (serial) way.
    /// </summary>
    public class AsyncSerialExecutor<TResult>
    {
        private readonly ConcurrentQueue<Tuple<TaskCompletionSource<TResult>, Func<Task<TResult>>>> _actions;
        private readonly InterlockedExchangeLock _locker;

        public AsyncSerialExecutor()
        {
            _actions = new ConcurrentQueue<Tuple<TaskCompletionSource<TResult>, Func<Task<TResult>>>>();
            _locker = new InterlockedExchangeLock();
        }

        /// <summary>
        /// Submit the next function for execution. It will execute after all previously submitted functions have finished, without interleaving their executions.
        /// Returns a promise that represents the execution of this given function. 
        /// The returned promise will be resolved when the given function is done executing.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public Task<TResult> AddNextAsync(Func<Task<TResult>> func)
        {
            var resolver = new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);

            _actions.Enqueue(new Tuple<TaskCompletionSource<TResult>, Func<Task<TResult>>>(resolver, func));

           var task = resolver.Task;

            ExecuteNextAsync().Ignore();

            return task;
        }

        private async Task ExecuteNextAsync()
        {
            while (!_actions.IsEmpty)
            {
                var gotLock = false;
                try
                {
                    if (!(gotLock = _locker.TryGetLock()))
                    {
                        return;
                    }

                    while (!_actions.IsEmpty)
                    {
                        if (!_actions.TryDequeue(out var actionTuple)) continue;

                        try
                        {
                            var result = await actionTuple.Item2();

                            actionTuple.Item1.TrySetResult(result);
                        }
                        catch (Exception exc)
                        {
                            actionTuple.Item1.TrySetException(exc);
                        }
                    }
                }
                finally
                {
                    if (gotLock)
                        _locker.ReleaseLock();
                }
            }
        }
    }

    public class AsyncSerialExecutor
    {
        private readonly AsyncSerialExecutor<bool> _executor = new AsyncSerialExecutor<bool>();

        public Task AddNext(Func<Task> func)
        {
            return _executor.AddNextAsync(() => WrapAsync(func));
        }

        private async Task<bool> WrapAsync(Func<Task> func)
        {
            await func();

            return true;
        }
    }
}
