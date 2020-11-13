using MDA.Infrastructure.Concurrent;
using MDA.Infrastructure.DataStructures;
using MDA.Infrastructure.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Infrastructure.Scheduling
{
    public sealed class HashedWheelTimer : ITimer
    {
        static readonly ILogger Logger = InternalLoggerFactory.GetInstance<HashedWheelTimer>();

        static int _instanceCounter;
        static int _warnedTooManyInstances;

        const int InstanceCountLimit = 64;

        readonly Worker _worker;
        readonly XThread _workerThread;
        readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        const int WorkerStateInit = 0;
        const int WorkerStateStarted = 1;
        const int WorkerStateShutdown = 2;
        int _workerStateVolatile = WorkerStateInit;

        readonly long _tickDuration;
        readonly HashedWheelBucket[] _wheel;
        readonly int _mask;
        readonly CountdownEvent _startTimeInitialized = new CountdownEvent(1);
        readonly IQueue<HashedWheelTimeout> _timeouts = new CompatibleConcurrentQueue<HashedWheelTimeout>();
        readonly IQueue<HashedWheelTimeout> _cancelledTimeouts = new CompatibleConcurrentQueue<HashedWheelTimeout>();
        internal long PendingTimeouts;
        readonly long _maxPendingTimeouts;
        long _startTimeVolatile;

        /// <summary>
        /// Creates a new timer.
        /// </summary>
        public HashedWheelTimer()
            : this(TimeSpan.FromMilliseconds(100))
        {
        }

        /// <summary>
        /// Creates a new timer.
        /// </summary>
        /// <param name="tickInterval">the interval between two consecutive ticks</param>
        /// <param name="ticksPerWheel">the size of the wheel</param>
        /// <param name="maxPendingTimeouts">The maximum number of pending timeouts after which call to
        /// <c>newTimeout</c> will result in <see cref="RejectedExecutionException"/> being thrown.
        /// No maximum pending timeouts limit is assumed if this value is 0 or negative.</param>
        /// <exception cref="ArgumentException">if either of <c>tickInterval</c> and <c>ticksPerWheel</c> is &lt;= 0</exception>
        public HashedWheelTimer(
            TimeSpan tickInterval,
            int ticksPerWheel = 512,
            long maxPendingTimeouts = -1)
        {
            if (tickInterval <= TimeSpan.Zero)
            {
                throw new ArgumentException($"{nameof(tickInterval)} must be greater than 0: {tickInterval}");
            }
            if (Math.Ceiling(tickInterval.TotalMilliseconds) > int.MaxValue)
            {
                throw new ArgumentException($"{nameof(tickInterval)} must be less than or equal to ${int.MaxValue} ms.");
            }
            if (ticksPerWheel <= 0)
            {
                throw new ArgumentException($"{nameof(ticksPerWheel)} must be greater than 0: {ticksPerWheel}");
            }
            if (ticksPerWheel > int.MaxValue / 2 + 1)
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(ticksPerWheel)} may not be greater than 2^30: {ticksPerWheel}");
            }

            // Normalize ticksPerWheel to power of two and initialize the wheel.
            this._wheel = CreateWheel(ticksPerWheel);
            this._worker = new Worker(this);
            this._mask = this._wheel.Length - 1;

            this._tickDuration = tickInterval.Ticks;

            // Prevent overflow
            if (this._tickDuration >= long.MaxValue / this._wheel.Length)
            {
                throw new ArgumentException(
                    $"tickInterval: {tickInterval} (expected: 0 < tickInterval in nanos < {long.MaxValue / this._wheel.Length}");
            }
            this._workerThread = new XThread(st => this._worker.Run());

            this._maxPendingTimeouts = maxPendingTimeouts;

            if (Interlocked.Increment(ref _instanceCounter) > InstanceCountLimit &&
                Interlocked.CompareExchange(ref _warnedTooManyInstances, 1, 0) == 0)
            {
                ReportTooManyInstances();
            }
        }

        ~HashedWheelTimer()
        {
            // This object is going to be GCed and it is assumed the ship has sailed to do a proper shutdown. If
            // we have not yet shutdown then we want to make sure we decrement the active instance count.
            if (Interlocked.Exchange(ref this._workerStateVolatile, WorkerStateShutdown) != WorkerStateShutdown)
            {
                Interlocked.Decrement(ref _instanceCounter);
            }
        }

        internal CancellationToken CancellationToken => this._cancellationTokenSource.Token;

        bool ShouldLimitTimeouts => this._maxPendingTimeouts > 0;

        PreciseTimeSpan StartTime
        {
            get => PreciseTimeSpan.FromTicks(Volatile.Read(ref this._startTimeVolatile));
            set => Volatile.Write(ref this._startTimeVolatile, value.Ticks);
        }

        int WorkerState => Volatile.Read(ref this._workerStateVolatile);

        static HashedWheelBucket[] CreateWheel(int ticksPerWheel)
        {
            ticksPerWheel = NormalizeTicksPerWheel(ticksPerWheel);
            var wheel = new HashedWheelBucket[ticksPerWheel];
            for (var i = 0; i < wheel.Length; i++)
            {
                wheel[i] = new HashedWheelBucket();
            }
            return wheel;
        }

        static int NormalizeTicksPerWheel(int ticksPerWheel)
        {
            int normalizedTicksPerWheel = 1;
            while (normalizedTicksPerWheel < ticksPerWheel)
            {
                normalizedTicksPerWheel <<= 1;
            }
            return normalizedTicksPerWheel;
        }

        /// <summary>
        /// Starts the background thread explicitly. The background thread will
        /// start automatically on demand even if you did not call this method.
        /// </summary>
        /// <exception cref="InvalidOperationException">if this timer has been
        /// stopped already.</exception>
        public void Start()
        {
            switch (this.WorkerState)
            {
                case WorkerStateInit:
                    if (Interlocked.CompareExchange(ref this._workerStateVolatile, WorkerStateStarted, WorkerStateInit) == WorkerStateInit)
                    {
                        this._workerThread.Start();
                    }
                    break;
                case WorkerStateStarted:
                    break;
                case WorkerStateShutdown:
                    throw new InvalidOperationException("cannot be started once stopped");
                default:
                    throw new InvalidOperationException("Invalid WorkerState");
            }

            // Wait until the startTime is initialized by the worker.
            if (this.StartTime == PreciseTimeSpan.Zero)
            {
                this._startTimeInitialized.Wait(this.CancellationToken);
            }
        }

        public async Task<ISet<ITimeout>> StopAsync(CancellationToken cancellationToken = default)
        {
            GC.SuppressFinalize(this);

            if (XThread.CurrentThread == this._workerThread)
            {
                throw new InvalidOperationException($"{nameof(HashedWheelTimer)}.stop() cannot be called from timer task.");
            }

            if (Interlocked.CompareExchange(ref this._workerStateVolatile, WorkerStateShutdown, WorkerStateStarted) != WorkerStateStarted)
            {
                // workerState can be 0 or 2 at this moment - let it always be 2.
                if (Interlocked.Exchange(ref this._workerStateVolatile, WorkerStateShutdown) != WorkerStateShutdown)
                {
                    this._cancellationTokenSource.Cancel();
                    Interlocked.Decrement(ref _instanceCounter);
                }

                return new HashSet<ITimeout>();
            }

            try
            {
                this._cancellationTokenSource.Cancel();
            }
            finally
            {
                Interlocked.Decrement(ref _instanceCounter);
            }
            await this._worker.ClosedFuture;
            return this._worker.UnprocessedTimeouts;
        }

        public ITimeout NewTimeout(ITimerTask task, TimeSpan delay)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }
            if (this.WorkerState == WorkerStateShutdown)
            {
                throw new RejectedExecutionException("Timer has been stopped and cannot process new operations.");
            }
            if (this.ShouldLimitTimeouts)
            {
                var pendingTimeoutsCount = Interlocked.Increment(ref this.PendingTimeouts);
                if (pendingTimeoutsCount > this._maxPendingTimeouts)
                {
                    Interlocked.Decrement(ref this.PendingTimeouts);
                    throw new RejectedExecutionException($"Number of pending timeouts ({pendingTimeoutsCount}) is greater than or equal to maximum allowed pending timeouts ({this._maxPendingTimeouts})");
                }
            }

            this.Start();

            // Add the timeout to the timeout queue which will be processed on the next tick.
            // During processing all the queued HashedWheelTimeouts will be added to the correct HashedWheelBucket.
            var deadline = CeilTimeSpanToMilliseconds((PreciseTimeSpan.Deadline(delay) - this.StartTime).ToTimeSpan());
            var timeout = new HashedWheelTimeout(this, task, deadline);
            this._timeouts.TryEnqueue(timeout);
            return timeout;
        }

        void ScheduleCancellation(HashedWheelTimeout timeout)
        {
            if (this.WorkerState != WorkerStateShutdown)
            {
                this._cancelledTimeouts.TryEnqueue(timeout);
            }
        }

        static void ReportTooManyInstances() =>
            Logger.LogError($"You are creating too many {nameof(HashedWheelTimer)} instances. {nameof(HashedWheelTimer)} is a shared resource that must be reused across the process,so that only a few instances are created.");

        static TimeSpan CeilTimeSpanToMilliseconds(TimeSpan time)
        {
            var remainder = time.Ticks % TimeSpan.TicksPerMillisecond;
            return remainder == 0 ? time : new TimeSpan(time.Ticks - remainder + TimeSpan.TicksPerMillisecond);
        }

        sealed class Worker : IRunnable
        {
            readonly HashedWheelTimer _owner;

            long _tick;
            readonly TaskCompletionSource _closedPromise;

            public Worker(HashedWheelTimer owner)
            {
                this._owner = owner;
                this._closedPromise = new TaskCompletionSource();
            }

            public Task ClosedFuture => this._closedPromise.Task;

            public void Run()
            {
                try
                {
                    // Initialize the startTime.
                    this._owner.StartTime = PreciseTimeSpan.FromStart;
                    if (this._owner.StartTime == PreciseTimeSpan.Zero)
                    {
                        // We use 0 as an indicator for the uninitialized value here, so make sure it's not 0 when initialized.
                        this._owner.StartTime = PreciseTimeSpan.FromTicks(1);
                    }

                    // Notify the other threads waiting for the initialization at start().
                    this._owner._startTimeInitialized.Signal();

                    while (true)
                    {
                        TimeSpan deadline = this.WaitForNextTick();
                        if (Volatile.Read(ref this._owner._workerStateVolatile) != WorkerStateStarted)
                        {
                            break;
                        }
                        if (deadline > TimeSpan.Zero)
                        {
                            int idx = (int)(this._tick & this._owner._mask);
                            this.ProcessCancelledTasks();
                            HashedWheelBucket bucket = this._owner._wheel[idx];
                            this.TransferTimeoutsToBuckets();
                            bucket.ExpireTimeouts(deadline);
                            this._tick++;
                        }
                    }

                    // Fill the unprocessedTimeouts so we can return them from stop() method.
                    foreach (HashedWheelBucket bucket in this._owner._wheel)
                    {
                        bucket.ClearTimeouts(this.UnprocessedTimeouts);
                    }
                    while (this._owner._timeouts.TryDequeue(out var timeout))
                    {
                        if (!timeout.Canceled)
                        {
                            this.UnprocessedTimeouts.Add(timeout);
                        }
                    }
                    this.ProcessCancelledTasks();
                }
                catch (Exception ex)
                {
                    Logger.LogError("Timeout processing failed.", ex);
                }
                finally
                {
                    this._closedPromise.TryComplete();
                }
            }

            void TransferTimeoutsToBuckets()
            {
                // transfer only max. 100000 timeouts per tick to prevent a thread to stall the workerThread when it just
                // adds new timeouts in a loop.
                for (var i = 0; i < 100000; i++)
                {
                    if (!this._owner._timeouts.TryDequeue(out var timeout))
                    {
                        // all processed
                        break;
                    }
                    if (timeout.State == HashedWheelTimeout.StCanceled)
                    {
                        // Was cancelled in the meantime.
                        continue;
                    }

                    var calculated = (timeout.Deadline.Ticks + this._owner._tickDuration - 1) / this._owner._tickDuration; // ceiling to timeout later rather than earlier
                    timeout.RemainingRounds = (calculated - this._tick) / this._owner._wheel.Length;

                    var ticks = Math.Max(calculated, this._tick); // Ensure we don't schedule for past.
                    var stopIndex = (int)(ticks & this._owner._mask);

                    HashedWheelBucket bucket = this._owner._wheel[stopIndex];
                    bucket.AddTimeout(timeout);
                }
            }

            void ProcessCancelledTasks()
            {
                while (true)
                {
                    if (!this._owner._cancelledTimeouts.TryDequeue(out var timeout))
                    {
                        // all processed
                        break;
                    }
                    try
                    {
                        timeout.Remove();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning("An exception was thrown while processing a cancellation task", ex);
                    }
                }
            }

            /// <summary>
            /// calculate timer firing time from startTime and current tick number,
            /// then wait until that goal has been reached.
            /// </summary>
            /// <returns>long.MinValue if received a shutdown request,
            /// current time otherwise (with long.MinValue changed by +1)
            /// </returns>
            TimeSpan WaitForNextTick()
            {
                var deadline = this._owner._tickDuration * (this._tick + 1);

                while (true)
                {
                    var currentTime = (PreciseTimeSpan.FromStart - this._owner.StartTime).ToTimeSpan();
                    var sleepTime = CeilTimeSpanToMilliseconds(TimeSpan.FromTicks(deadline - currentTime.Ticks));

                    if (sleepTime <= TimeSpan.Zero)
                    {
                        return currentTime.Ticks == long.MinValue
                            ? TimeSpan.FromTicks(-long.MaxValue)
                            : currentTime;
                    }

                    Task delay = null;
                    try
                    {
                        var sleepTimeMs = sleepTime.Ticks / TimeSpan.TicksPerMillisecond; // we've already rounded so no worries about the remainder > 0 here
                        Contract.Assert(sleepTimeMs <= int.MaxValue);
                        delay = Task.Delay((int)sleepTimeMs, this._owner.CancellationToken);
                        delay.Wait();
                    }
                    catch (AggregateException) when (delay != null && delay.IsCanceled)
                    {
                        if (Volatile.Read(ref this._owner._workerStateVolatile) == WorkerStateShutdown)
                        {
                            return TimeSpan.FromTicks(long.MinValue);
                        }
                    }
                }
            }

            internal ISet<ITimeout> UnprocessedTimeouts { get; } = new HashSet<ITimeout>();
        }

        sealed class HashedWheelTimeout : ITimeout
        {
            const int StInit = 0;
            internal const int StCanceled = 1;
            const int StExpired = 2;

            internal readonly HashedWheelTimer timer;
            internal readonly TimeSpan Deadline;

            volatile int _state = StInit;

            // remainingRounds will be calculated and set by Worker.transferTimeoutsToBuckets() before the
            // HashedWheelTimeout will be added to the correct HashedWheelBucket.
            internal long RemainingRounds;

            // This will be used to chain timeouts in HashedWheelTimerBucket via a double-linked-list.
            // As only the workerThread will act on it there is no need for synchronization / volatile.
            internal HashedWheelTimeout Next;

            internal HashedWheelTimeout Prev;

            // The bucket to which the timeout was added
            internal HashedWheelBucket Bucket;

            internal HashedWheelTimeout(HashedWheelTimer timer, ITimerTask task, TimeSpan deadline)
            {
                this.timer = timer;
                this.Task = task;
                this.Deadline = deadline;
            }

            public ITimer Timer => this.timer;

            public ITimerTask Task { get; }

            public bool Cancel()
            {
                // only update the state it will be removed from HashedWheelBucket on next tick.
                if (!this.CompareAndSetState(StInit, StCanceled))
                {
                    return false;
                }
                // If a task should be canceled we put this to another queue which will be processed on each tick.
                // So this means that we will have a GC latency of max. 1 tick duration which is good enough. This way
                // we can make again use of our MpscLinkedQueue and so minimize the locking / overhead as much as possible.
                this.timer.ScheduleCancellation(this);
                return true;
            }

            internal void Remove()
            {
                HashedWheelBucket bucket = this.Bucket;
                if (bucket != null)
                {
                    // timeout got canceled before it was added to the bucket
                    bucket.Remove(this);
                }
                else if (this.timer.ShouldLimitTimeouts)
                {
                    Interlocked.Decrement(ref this.timer.PendingTimeouts);
                }
            }

            bool CompareAndSetState(int expected, int state)
            {
                return Interlocked.CompareExchange(ref this._state, state, expected) == expected;
            }

            internal int State => this._state;

            public bool Canceled => this.State == StCanceled;

            public bool Expired => this.State == StExpired;

            internal void Expire()
            {
                if (!this.CompareAndSetState(StInit, StExpired))
                {
                    return;
                }

                try
                {
                    this.Task.Run(this);
                }
                catch (Exception t)
                {
                    Logger.LogWarning($"An exception was thrown by {this.Task.GetType().Name}.", t);
                }
            }

            public override string ToString()
            {
                var currentTime = PreciseTimeSpan.FromStart - this.timer.StartTime;
                var remaining = this.Deadline - currentTime.ToTimeSpan();

                var buf = new StringBuilder(192)
                    .Append(this.GetType().Name)
                    .Append('(')
                    .Append("deadline: ");
                if (remaining > TimeSpan.Zero)
                {
                    buf.Append(remaining)
                        .Append(" later");
                }
                else if (remaining < TimeSpan.Zero)
                {
                    buf.Append(-remaining)
                        .Append(" ago");
                }
                else
                {
                    buf.Append("now");
                }

                if (this.Canceled)
                {
                    buf.Append(", cancelled");
                }

                return buf.Append(", task: ")
                    .Append(this.Task)
                    .Append(')')
                    .ToString();
            }
        }

        /// <summary>
        /// Bucket that stores HashedWheelTimeouts. These are stored in a linked-list like datastructure to allow easy
        /// removal of HashedWheelTimeouts in the middle. Also the HashedWheelTimeout act as nodes themself and so no
        /// extra object creation is needed.
        /// </summary>
        sealed class HashedWheelBucket
        {
            // Used for the linked-list datastructure
            HashedWheelTimeout _head;
            HashedWheelTimeout _tail;

            /// <summary>
            /// Add {@link HashedWheelTimeout} to this bucket.
            /// </summary>
            public void AddTimeout(HashedWheelTimeout timeout)
            {
                Contract.Assert(timeout.Bucket == null);
                timeout.Bucket = this;
                if (this._head == null)
                {
                    this._head = this._tail = timeout;
                }
                else
                {
                    this._tail.Next = timeout;
                    timeout.Prev = this._tail;
                    this._tail = timeout;
                }
            }

            /// <summary>
            /// Expire all <see cref="HashedWheelTimeout"/>s for the given <c>deadline</c>.
            /// </summary>
            public void ExpireTimeouts(TimeSpan deadline)
            {
                HashedWheelTimeout timeout = this._head;

                // process all timeouts
                while (timeout != null)
                {
                    HashedWheelTimeout next = timeout.Next;
                    if (timeout.RemainingRounds <= 0)
                    {
                        next = this.Remove(timeout);
                        if (timeout.Deadline <= deadline)
                        {
                            timeout.Expire();
                        }
                        else
                        {
                            // The timeout was placed into a wrong slot. This should never happen.
                            throw new InvalidOperationException(
                                string.Format(
                                    "timeout.deadline (%d) > deadline (%d)",
                                    timeout.Deadline,
                                    deadline));
                        }
                    }
                    else
                    {
                        timeout.RemainingRounds--;
                    }
                    timeout = next;
                }
            }

            public HashedWheelTimeout Remove(HashedWheelTimeout timeout)
            {
                HashedWheelTimeout next = timeout.Next;
                // remove timeout that was either processed or cancelled by updating the linked-list
                if (timeout.Prev != null)
                {
                    timeout.Prev.Next = next;
                }
                if (timeout.Next != null)
                {
                    timeout.Next.Prev = timeout.Prev;
                }

                if (timeout == this._head)
                {
                    // if timeout is also the tail we need to adjust the entry too
                    if (timeout == this._tail)
                    {
                        this._tail = null;
                        this._head = null;
                    }
                    else
                    {
                        this._head = next;
                    }
                }
                else if (timeout == this._tail)
                {
                    // if the timeout is the tail modify the tail to be the prev node.
                    this._tail = timeout.Prev;
                }
                // null out prev, next and bucket to allow for GC.
                timeout.Prev = null;
                timeout.Next = null;
                timeout.Bucket = null;
                if (timeout.timer.ShouldLimitTimeouts)
                {
                    Interlocked.Decrement(ref timeout.timer.PendingTimeouts);
                }
                return next;
            }

            /// <summary>
            /// Clear this bucket and return all not expired / cancelled <see cref="ITimeout"/>s.
            /// </summary>
            public void ClearTimeouts(ISet<ITimeout> set)
            {
                while (true)
                {
                    var timeout = this.PollTimeout();
                    if (timeout == null)
                    {
                        return;
                    }
                    if (timeout.Expired || timeout.Canceled)
                    {
                        continue;
                    }
                    set.Add(timeout);
                }
            }

            HashedWheelTimeout PollTimeout()
            {
                HashedWheelTimeout head = this._head;
                if (head == null)
                {
                    return null;
                }
                HashedWheelTimeout next = head.Next;
                if (next == null)
                {
                    this._tail = this._head = null;
                }
                else
                {
                    this._head = next;
                    next.Prev = null;
                }

                // null out prev and next to allow for GC.
                head.Next = null;
                head.Prev = null;
                head.Bucket = null;
                return head;
            }
        }
    }
}
