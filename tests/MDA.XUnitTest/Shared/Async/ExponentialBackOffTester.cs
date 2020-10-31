using System;
using MDA.Infrastructure.Async;
using Xunit;
using Xunit.Abstractions;

namespace MDA.XUnitTest.Shared.Async
{
    public class ExponentialBackOffTester
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ExponentialBackOffTester(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void RandomStepExponentialBackOffTest()
        {
            var minDelay = TimeSpan.FromSeconds(1);
            var maxDelay = TimeSpan.FromMinutes(10);
            var step = TimeSpan.FromSeconds(1);
            var backOff = new RandomStepExponentialBackOff(minDelay, maxDelay, step);

            for (var i = 1; i <= 5; i++)
            {
                _testOutputHelper.WriteLine("Running for {0} time, delay: " + backOff.Next(i), i);
            }
        }

        [Fact]
        public void FixedStepExponentialBackOffTest_1()
        {
            var minDelay = TimeSpan.FromSeconds(1);
            var maxDelay = TimeSpan.FromMinutes(10);
            var step = TimeSpan.FromSeconds(1);
            var backOff = new FixedStepExponentialBackOff(minDelay, maxDelay, step);

            for (var i = 1; i <= 5; i++)
            {
                _testOutputHelper.WriteLine("Running for {0} time, delay: " + backOff.Next(i), i);
            }
        }

        [Fact]
        public void FixedStepExponentialBackOffTest_2()
        {
            var minDelay = TimeSpan.FromSeconds(3);
            var maxDelay = TimeSpan.FromSeconds(20);
            var step = TimeSpan.FromSeconds(1);
            var backOff = new FixedStepExponentialBackOff(minDelay, maxDelay, step);

            for (var i = 1; i <= 5; i++)
            {
                _testOutputHelper.WriteLine("Running for {0} time, delay: " + backOff.Next(i), i);
            }
        }
    }
}
