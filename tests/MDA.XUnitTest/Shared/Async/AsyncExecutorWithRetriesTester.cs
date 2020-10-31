using MDA.Infrastructure.Async;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MDA.XUnitTest.Shared.Async
{
    public class AsyncExecutorWithRetriesTester
    {
        private readonly ITestOutputHelper _output;

        public AsyncExecutorWithRetriesTester(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact, TestCategory("Functional"), TestCategory("AsynchronyPrimitives")]
        public void Async_AsyncExecutorWithRetriesTest_1()
        {
            var counter = 0;

            Task<int> MyFunc(int funcCounter)
            {
                // ReSharper disable AccessToModifiedClosure
                Assert.Equal(counter, funcCounter);
                _output.WriteLine("Running for {0} time.", counter);
                counter++;

                return counter == 5 ? Task.FromResult(28) 
                    : throw new ArgumentException("Wrong arg!");
                // ReSharper restore AccessToModifiedClosure
            }

            bool ErrorFilter(Exception exc, int i)
            {
                return true;
            }

            var promise = AsyncExecutorWithRetries.ExecuteWithRetriesAsync(
                MyFunc, 
                10, 
                10, 
                null, 
                ErrorFilter);
            var value = promise.Result;
            _output.WriteLine("Value is {0}.", value);

            counter = 0;
            try
            {
                promise = AsyncExecutorWithRetries.ExecuteWithRetriesAsync(
                    MyFunc,
                    3,
                    3,
                    null,
                    ErrorFilter);
                value = promise.Result;
                _output.WriteLine("Value is {0}.", value);
            }
            catch (Exception)
            {
                return;
            }

            Assert.True(false, "Should have thrown");
        }

        [Fact, TestCategory("Functional"), TestCategory("AsynchronyPrimitives")]
        public void Async_AsyncExecutorWithRetriesTest_2()
        {
            var counter = 0;
            const int countLimit = 5;

            Task<int> MyFunc(int funcCounter)
            {
                // ReSharper disable AccessToModifiedClosure
                Assert.Equal(counter, funcCounter);
                _output.WriteLine("Running for {0} time.", counter);
                return Task.FromResult(++counter);
                // ReSharper restore AccessToModifiedClosure
            }

            bool SuccessFilter(int count, int i) => count != countLimit;

            var maxRetries = 10;
            var expectedRetries = countLimit;
            var promise = AsyncExecutorWithRetries.ExecuteWithRetriesAsync(
                MyFunc,
                maxRetries, 
                maxRetries, 
                SuccessFilter, 
                null,
                Timeout.InfiniteTimeSpan);
            var value = promise.Result;
            _output.WriteLine("Value={0} Counter={1} ExpectedRetries={2}", value, counter, expectedRetries);
            Assert.Equal(expectedRetries, value); // "Returned value"
            Assert.Equal(counter, value); // "Counter == Returned value"

            counter = 0;
            maxRetries = 3;
            expectedRetries = maxRetries;
            promise = AsyncExecutorWithRetries.ExecuteWithRetriesAsync(
                MyFunc, 
                maxRetries, 
                maxRetries, 
                SuccessFilter, 
                null);
            value = promise.Result;
            _output.WriteLine("Value={0} Counter={1} ExpectedRetries={2}", value, counter, expectedRetries);
            Assert.Equal(expectedRetries, value); // "Returned value"
            Assert.Equal(counter, value); // "Counter == Returned value"
        }

        [Fact, TestCategory("Functional"), TestCategory("AsynchronyPrimitives")]
        public void Async_AsyncExecutorWithRetriesTest_3()
        {
            var counter = 0;
            var lastIteration = 0;

            Task<int> MyFunc(int funcCounter)
            {
                lastIteration = funcCounter;
                Assert.Equal(counter, funcCounter);
                _output.WriteLine("Running for {0} time.", counter);
                return Task.FromResult(++counter);
            }

            bool ErrorFilter(Exception exc, int i)
            {
                Assert.Equal(lastIteration, i);
                Assert.True(false, "Should not be called");
                return true;
            }

            var maxRetries = 5;
            var promise = AsyncExecutorWithRetries.ExecuteWithRetriesAsync(
                MyFunc,
                maxRetries,
                ErrorFilter,
                default,
                new FixedBackOff(TimeSpan.FromSeconds(1)));

            var value = promise.Result;
            _output.WriteLine("Value={0} Counter={1} ExpectedRetries={2}", value, counter, 0);
            Assert.Equal(counter, value);
            Assert.Equal(1, counter);
        }

        [Fact, TestCategory("Functional"), TestCategory("AsynchronyPrimitives")]
        public void Async_AsyncExecutorWithRetriesTest_4()
        {
            var counter = 0;
            var lastIteration = 0;

            Task<int> MyFunc(int funcCounter)
            {
                lastIteration = funcCounter;
                Assert.Equal(counter, funcCounter);
                _output.WriteLine("Running FUNC for {0} time.", counter);
                ++counter;
                throw new ArgumentException(counter.ToString(CultureInfo.InvariantCulture));
            }

            bool ErrorFilter(Exception exc, int i)
            {
                _output.WriteLine("Running ERROR FILTER for {0} time.", i);
                Assert.Equal(lastIteration, i);
                if (i == 0 || i == 1)
                    return true;
                else if (i == 2)
                    throw exc;
                else
                    return false;
            }

            var maxRetries = 5;
            var promise = AsyncExecutorWithRetries.ExecuteWithRetriesAsync(
                MyFunc,
                maxRetries,
                ErrorFilter,
                default,
                new FixedBackOff(TimeSpan.FromSeconds(1)));
            try
            {
                var value = promise.Result;
                Assert.True(false, "Should have thrown");
            }
            catch (Exception exc)
            {
                var baseExc = exc.GetBaseException();
                Assert.Equal(typeof(ArgumentException), baseExc.GetType());
                _output.WriteLine("baseExc.GetType()={0} Counter={1}", baseExc.GetType(), counter);
                Assert.Equal(3, counter); // "Counter == Returned value"
            }
        }
    }
}
