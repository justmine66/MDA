using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MDA.XUnitTest.Shared.Async
{
    public class TaskCreationOptionsTester
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TaskCreationOptionsTester(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task TestRunContinuationsAsynchronously()
        {
            ThreadPool.SetMinThreads(100, 100);

            _testOutputHelper.WriteLine("Main CurrentManagedThreadId:" + Environment.CurrentManagedThreadId);

            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            //  使用TaskContinuationOptions.ExecuteSynchronously来测试延续任务
            await ContinueWithAsync(1, tcs.Task);

            //  测试await延续任务
            await ContinueAsync(2, tcs.Task);

            await Task.Run(() =>
            {
                _testOutputHelper.WriteLine("Task Run CurrentManagedThreadId:" + Environment.CurrentManagedThreadId);
                tcs.TrySetResult(true);
            });
        }

        static void Print(int id) => Console.WriteLine($"continuation:{id}\tCurrentManagedThread:{Environment.CurrentManagedThreadId}");

        static async Task ContinueAsync(int id, Task task)
        {
            await task.ConfigureAwait(false);

            Print(id);
        }
        static Task ContinueWithAsync(int id, Task task)
        {
            return task.ContinueWith(
                t => Print(id),
                CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }
    }
}
