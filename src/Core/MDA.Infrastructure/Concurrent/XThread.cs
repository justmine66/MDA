using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Infrastructure.Concurrent
{
    public delegate void XParameterizedThreadStart(object obj);

    [DebuggerDisplay("ID={_threadId}, Name={Name}, IsExplicit={_isExplicit}")]
    public sealed class XThread
    {
        static int _maxThreadId;

        [ThreadStatic]
        static XThread _currentThread;

        readonly int _threadId;
#pragma warning disable CS0414
        readonly bool _isExplicit; // For debugging only
#pragma warning restore CS0414
        Task _task;
        readonly EventWaitHandle _completed = new EventWaitHandle(false, EventResetMode.AutoReset);
        readonly EventWaitHandle _readyToStart = new EventWaitHandle(false, EventResetMode.AutoReset);
        object _startupParameter;

        static int GetNewThreadId() => Interlocked.Increment(ref _maxThreadId);

        XThread()
        {
            _threadId = GetNewThreadId();
            _isExplicit = false;
            IsAlive = false;
        }

        public XThread(Action action)
        {
            _threadId = GetNewThreadId();
            _isExplicit = true;
            IsAlive = false;

            CreateLongRunningTask(x => action());
        }

        public XThread(XParameterizedThreadStart threadStartFunc)
        {
            _threadId = GetNewThreadId();
            _isExplicit = true;
            IsAlive = false;

            CreateLongRunningTask(threadStartFunc);
        }

        public void Start()
        {
            _readyToStart.Set();
            IsAlive = true;
        }

        void CreateLongRunningTask(XParameterizedThreadStart threadStartFunc)
        {
            _task = Task.Factory.StartNew(
                () =>
                {
                    // We start the task running, then unleash it by signaling the readyToStart event.
                    // This is needed to avoid thread reuse for tasks (see below)
                    _readyToStart.WaitOne();
                    // This is the first time we're using this thread, therefore the TLS slot must be empty
                    if (_currentThread != null)
                    {
                        Debug.WriteLine("warning: currentThread already created; OS thread reused");
                        Debug.Assert(false);
                    }
                    _currentThread = this;
                    threadStartFunc(_startupParameter);
                    _completed.Set();
                },
                CancellationToken.None,
                // .NET always creates a brand new thread for LongRunning tasks
                // This is not documented but unlikely to ever change:
                // https://github.com/dotnet/corefx/issues/2576#issuecomment-126693306
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

        public void Start(object parameter)
        {
            _startupParameter = parameter;
            Start();
        }

        public static void Sleep(int millisecondsTimeout) => Task.Delay(millisecondsTimeout).Wait();

        public int Id => _threadId;

        public string Name { get; set; }

        public bool IsAlive { get; private set; }

        public static XThread CurrentThread => _currentThread ??= new XThread();

        public bool Join(TimeSpan timeout) => _completed.WaitOne(timeout);

        public bool Join(int millisecondsTimeout) => _completed.WaitOne(millisecondsTimeout);
    }
}
