using MDA.Infrastructure.Async;
using MDA.Infrastructure.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MDA.XUnitTest.Shared.Async
{
    public class AsyncSerialExecutorTester
    {
        private readonly ITestOutputHelper _output;
        private SafeRandom _random;
        private int _operationsInProgress;

        public AsyncSerialExecutorTester(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact, TestCategory("Functional"), TestCategory("Async")]
        public async Task AsyncSerialExecutorTests_Small()
        {
            var executor = new AsyncSerialExecutor();
            var tasks = new List<Task>();
            _random = new SafeRandom();
            _operationsInProgress = 0;

            tasks.Add(executor.AddNext(() => Operation(1)));
            tasks.Add(executor.AddNext(() => Operation(2)));
            tasks.Add(executor.AddNext(() => Operation(3)));

            await Task.WhenAll(tasks);
        }

        [Fact, TestCategory("Functional"), TestCategory("Async")]
        public async Task AsyncSerialExecutorTests_SerialSubmit()
        {
            var executor = new AsyncSerialExecutor();

            _random = new SafeRandom();
            var tasks = new List<Task>();

            for (var i = 0; i < 10; i++)
            {
                var capture = i;
                _output.WriteLine("Submitting Task {0}.", capture);
                tasks.Add(executor.AddNext(() => Operation(capture)));
            }

            await Task.WhenAll(tasks);
        }

        [Fact, TestCategory("Functional"), TestCategory("Async")]
        public async Task AsyncSerialExecutorTests_ParallelSubmit()
        {
            _random = new SafeRandom();

            var executor = new AsyncSerialExecutor();
            var tasks = new ConcurrentStack<Task>();
            var enqueueTasks = new List<Task>();

            for (var i = 0; i < 10; i++)
            {
                var capture = i;
                enqueueTasks.Add(
                    Task.Run(() =>
                    {
                        _output.WriteLine("Submitting Task {0}.", capture);

                        tasks.Push(executor.AddNext(() => Operation(capture)));
                    }));
            }

            await Task.WhenAll(enqueueTasks);
            await Task.WhenAll(tasks);
        }

        private async Task Operation(int opNumber)
        {
            if (_operationsInProgress > 0)
                Assert.True(false, $"1: Operation {opNumber} found {_operationsInProgress} operationsInProgress.");
            _operationsInProgress++;
            var delay = _random.NextTimeSpan(TimeSpan.FromSeconds(2));

            _output.WriteLine("Task {0} Staring, delay: {1}", opNumber, delay);
            await Task.Delay(delay);
            if (_operationsInProgress != 1)
                Assert.True(false, $"2: Operation {opNumber} found {_operationsInProgress} operationsInProgress.");

            _output.WriteLine("Task {0} after first delay", opNumber);
            await Task.Delay(delay);
            if (_operationsInProgress != 1)
                Assert.True(false, $"3: Operation {opNumber} found {_operationsInProgress} operationsInProgress.");

            _operationsInProgress--;
            _output.WriteLine("Task {0} Done", opNumber);
        }
    }
}
