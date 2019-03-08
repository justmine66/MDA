using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Common.Concurrent
{
    public class ScheduledExecutorImpl : IScheduledExecutor
    {
        private int _periodNo = 1;

        public ICancellable Schedule(IRunnable command, TimeSpan delay)
        {
            Assert.NotNull(command, nameof(command));
            Assert.Positive(delay.Ticks, nameof(delay));

            var canceler = new TimedCanceler(delay, TimeSpan.FromMilliseconds(Timeout.Infinite), command);

            return canceler;
        }

        public ICancellable<TState> Schedule<TState>(IRunnable<TState> command, TimeSpan delay, TState state)
            where TState : class
        {
            Assert.NotNull(command, nameof(command));
            Assert.NotNull(state, nameof(state));
            Assert.Positive(delay.Ticks, nameof(delay));

            var canceler = new TimedCanceler<TState>(delay, TimeSpan.FromMilliseconds(Timeout.Infinite), command, state);

            return canceler;
        }

        public Task<ICancellable> ScheduleAsync(IAsyncRunnable command, TimeSpan delay)
        {
            var canceler = Schedule(command, delay);
            return Task.FromResult(canceler);
        }

        public Task<ICancellable<TState>> ScheduleAsync<TState>(IAsyncRunnable<TState> command, TimeSpan delay, TState state)
            where TState : class
        {
            var canceler = Schedule(command, delay, state);
            return Task.FromResult(canceler);
        }

        public ICancellable ScheduleAtFixedRate(IRunnable command, TimeSpan initialDelay, TimeSpan period)
        {
            Assert.NotNull(command, nameof(command));
            Assert.Positive(period.Ticks, nameof(period));

            var ticks = period.Ticks * _periodNo;
            var canceler = new TimedCanceler(initialDelay, TimeSpan.FromTicks(ticks), command);

            _periodNo++;

            return canceler;
        }

        public ICancellable<TState> ScheduleAtFixedRate<TState>(IRunnable<TState> command, TimeSpan initialDelay, TimeSpan period, TState state)
            where TState : class
        {
            Assert.NotNull(command, nameof(command));
            Assert.NotNull(state, nameof(state));
            Assert.Positive(period.Ticks, nameof(period));

            var ticks = period.Ticks * _periodNo;
            var canceler = new TimedCanceler<TState>(initialDelay, TimeSpan.FromTicks(ticks), command, state);

            _periodNo++;

            return canceler;
        }

        public Task<ICancellable> ScheduleAtFixedRateAsync(IAsyncRunnable command, TimeSpan initialDelay, TimeSpan period)
        {
            var canceler = ScheduleAtFixedRate(command, initialDelay, period);
            return Task.FromResult(canceler);
        }

        public Task<ICancellable<TState>> ScheduleAtFixedRateAsync<TState>(IAsyncRunnable<TState> command, TimeSpan initialDelay, TimeSpan period, TState state)
            where TState : class
        {
            var canceler = ScheduleAtFixedRate(command, initialDelay, period, state);
            return Task.FromResult(canceler);
        }

        public ICancellable ScheduleWithFixedDelay(IRunnable command, TimeSpan initialDelay, TimeSpan delay)
        {
            Assert.NotNull(command, nameof(command));
            Assert.Positive(delay.Ticks, nameof(delay));

            var canceler = new TimedCanceler(initialDelay, delay, command);

            return canceler;
        }

        public ICancellable<TState> ScheduleWithFixedDelay<TState>(IRunnable<TState> command, TimeSpan initialDelay, TimeSpan delay, TState state)
            where TState : class
        {
            Assert.NotNull(command, nameof(command));
            Assert.NotNull(state, nameof(state));
            Assert.Positive(delay.Ticks, nameof(delay));

            var canceler = new TimedCanceler<TState>(initialDelay, delay, command, state);

            return canceler;
        }

        public Task<ICancellable> ScheduleWithFixedDelayAsync(IAsyncRunnable command, TimeSpan initialDelay, TimeSpan delay)
        {
            var canceler = ScheduleWithFixedDelay(command, initialDelay, delay);
            return Task.FromResult(canceler);
        }

        public Task<ICancellable<TState>> ScheduleWithFixedDelayAsync<TState>(IAsyncRunnable<TState> command, TimeSpan initialDelay, TimeSpan delay,
            TState state)
            where TState : class
        {
            var canceler = ScheduleAtFixedRate(command, initialDelay, delay, state);
            return Task.FromResult(canceler);
        }
    }
}
