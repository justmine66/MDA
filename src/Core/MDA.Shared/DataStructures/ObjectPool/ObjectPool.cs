using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Shared.DataStructures.ObjectPool
{
    /// <summary>
    /// 对象池管理类
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public partial class ObjectPool<T> : IObjectPool<T>
    {
        public IPolicy<T> Policy { get; protected set; }

        private readonly List<Object<T>> _allObjects = new List<Object<T>>();
        private readonly object _allObjectsLock = new object();
        private readonly ConcurrentStack<Object<T>> _freeObjects = new ConcurrentStack<Object<T>>();

        private readonly ConcurrentQueue<GetSyncQueueInfo> _getSyncQueue = new ConcurrentQueue<GetSyncQueueInfo>();
        private readonly ConcurrentQueue<TaskCompletionSource<Object<T>>> _getAsyncQueue = new ConcurrentQueue<TaskCompletionSource<Object<T>>>();
        private readonly ConcurrentQueue<bool> _getQueue = new ConcurrentQueue<bool>();

        public bool IsAvailable => this.UnavailableException == null;
        public Exception UnavailableException { get; private set; }
        public DateTime? UnavailableTime { get; private set; }
        private readonly object _unavailableLock = new object();
        private bool _running = true;

        public bool SetUnavailable(Exception exception)
        {
            var set = false;

            if (exception != null && UnavailableException == null)
            {
                lock (_unavailableLock)
                {
                    if (UnavailableException == null)
                    {
                        UnavailableException = exception;
                        UnavailableTime = DateTime.Now;
                        set = true;
                    }
                }
            }

            if (set)
            {
                Policy.OnUnavailable();
                PeriodicallyCheckAvailable(Policy.CheckAvailableInterval);
            }

            return set;
        }

        /// <summary>
        /// 后台定时检查可用性
        /// </summary>
        /// <param name="interval"></param>
        private void PeriodicallyCheckAvailable(int interval)
        {
            new Thread(() =>
            {
                if (UnavailableException != null)
                {
                    var bgColor = Console.BackgroundColor;
                    var foreColor = Console.ForegroundColor;
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"【{Policy.Name}】恢复检查时间：{DateTime.Now.AddSeconds(interval)}");
                    Console.BackgroundColor = bgColor;
                    Console.ForegroundColor = foreColor;
                    Console.WriteLine();
                }

                while (UnavailableException != null)
                {
                    if (_running == false) return;

                    Thread.CurrentThread.Join(TimeSpan.FromSeconds(interval));

                    if (_running == false) return;

                    try
                    {
                        var conn = GetFree(false);
                        if (conn == null) throw new Exception($"CheckAvailable 无法获得资源，{this.Statistics}");

                        try
                        {

                            if (Policy.OnCheckAvailable(conn) == false)
                                throw new Exception("CheckAvailable 应抛出异常，代表仍然不可用。");

                            break;

                        }
                        finally
                        {

                            Return(conn);
                        }
                    }
                    catch (Exception ex)
                    {
                        var bgColor = Console.BackgroundColor;
                        var foreColor = Console.ForegroundColor;
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"【{Policy.Name}】仍然不可用，下一次恢复检查时间：{DateTime.Now.AddSeconds(interval)}，错误：({ex.Message})");
                        Console.BackgroundColor = bgColor;
                        Console.ForegroundColor = foreColor;
                        Console.WriteLine();
                    }
                }

                RestoreToAvailable();

            }).Start();
        }

        private void RestoreToAvailable()
        {

            bool isRestored = false;
            if (UnavailableException != null)
            {

                lock (_unavailableLock)
                {

                    if (UnavailableException != null)
                    {

                        UnavailableException = null;
                        UnavailableTime = null;
                        isRestored = true;
                    }
                }
            }

            if (isRestored)
            {

                lock (_allObjectsLock)
                    _allObjects.ForEach(a => a.LastGetTime = a.LastReturnTime = new DateTime(2000, 1, 1));

                Policy.OnAvailable();

                var bgcolor = Console.BackgroundColor;
                var forecolor = Console.ForegroundColor;
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"【{Policy.Name}】已恢复工作");
                Console.BackgroundColor = bgcolor;
                Console.ForegroundColor = forecolor;
                Console.WriteLine();
            }
        }

        protected bool LiveCheckAvailable()
        {

            try
            {

                var conn = GetFree(false);
                if (conn == null) throw new Exception($"LiveCheckAvailable 无法获得资源，{this.Statistics}");

                try
                {

                    if (Policy.OnCheckAvailable(conn) == false) throw new Exception("LiveCheckAvailable 应抛出异常，代表仍然不可用。");

                }
                finally
                {

                    Return(conn);
                }

            }
            catch
            {
                return false;
            }

            RestoreToAvailable();

            return true;
        }

        public string Statistics => $"Pool: {_freeObjects.Count}/{_allObjects.Count}, Get wait: {_getSyncQueue.Count}, GetAsync wait: {_getAsyncQueue.Count}";
        public string StatisticsInFull
        {
            get
            {
                var sb = new StringBuilder();

                sb.AppendLine(Statistics);
                sb.AppendLine("");

                foreach (var obj in _allObjects)
                {
                    sb.AppendLine($"{obj.Value}, Times: {obj.GetTimes}, ThreadId(R/G): {obj.LastReturnThreadId}/{obj.LastGetThreadId}, Time(R/G): {obj.LastReturnTime:yyyy-MM-dd HH:mm:ss:ms}/{obj.LastGetTime:yyyy-MM-dd HH:mm:ss:ms}, ");
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// 创建对象池
        /// </summary>
        /// <param name="poolSize">池大小</param>
        /// <param name="createObject">池内对象的创建委托</param>
        /// <param name="onGetObject">获取池内对象成功后，进行使用前操作</param>
        public ObjectPool(int poolSize,
            Func<T> createObject,
            Action<Object<T>> onGetObject = null)
            : this(new DefaultPolicy<T>
            {
                PoolSize = poolSize,
                CreateObject = createObject,
                OnGetObject = onGetObject
            })
        { }

        /// <summary>
        /// 创建对象池
        /// </summary>
        /// <param name="policy">策略</param>
        public ObjectPool(IPolicy<T> policy)
        {
            Policy = policy;

            AppDomain.CurrentDomain.ProcessExit += (s1, e1) =>
            {
                if (Policy.IsAutoDisposeWithSystem)
                    _running = false;
            };
            try
            {
                Console.CancelKeyPress += (s1, e1) =>
                {
                    if (e1.Cancel) return;
                    if (Policy.IsAutoDisposeWithSystem)
                        _running = false;
                };
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// 获取可用资源，或创建资源
        /// </summary>
        /// <returns></returns>
        private Object<T> GetFree(bool checkAvailable)
        {
            if (_running == false)
                throw new ObjectDisposedException($"【{Policy.Name}】对象池已释放，无法访问。");

            if (checkAvailable && UnavailableException != null)
                throw new Exception($"【{Policy.Name}】状态不可用，等待后台检查程序恢复方可使用。{UnavailableException?.Message}");

            if ((_freeObjects.TryPop(out var obj) == false || obj == null) && _allObjects.Count < Policy.PoolSize)
            {
                lock (_allObjectsLock)
                    if (_allObjects.Count < Policy.PoolSize)
                        _allObjects.Add(obj = new Object<T> { Pool = this, Id = _allObjects.Count + 1 });
            }

            if (obj != null)
                obj.IsReturned = false;

            if (obj != null && obj.Value == null ||
                obj != null && Policy.IdleTimeout > TimeSpan.Zero && DateTime.Now.Subtract(obj.LastReturnTime) > Policy.IdleTimeout)
            {
                try
                {
                    obj.ResetValue();
                }
                catch
                {
                    Return(obj);
                    throw;
                }
            }

            return obj;
        }

        public Object<T> Get(TimeSpan? timeout = null)
        {
            var obj = GetFree(true);

            if (obj == null)
            {
                var queueItem = new GetSyncQueueInfo();

                _getSyncQueue.Enqueue(queueItem);
                _getQueue.Enqueue(false);

                if (timeout == null) timeout = Policy.SyncGetTimeout;

                try
                {
                    if (queueItem.Wait.Wait(timeout.Value))
                        obj = queueItem.ReturnValue;
                }
                catch
                {
                    // ignored
                }

                if (obj == null) obj = queueItem.ReturnValue;
                if (obj == null) lock (queueItem.Lock) queueItem.IsTimeout = (obj = queueItem.ReturnValue) == null;
                if (obj == null) obj = queueItem.ReturnValue;

                if (obj == null)
                {
                    Policy.OnGetTimeout();

                    if (Policy.IsThrowGetTimeoutException)
                        throw new TimeoutException($"ObjectPool.Get 获取超时（{timeout.Value.TotalSeconds}秒）。");

                    return null;
                }
            }

            try
            {
                Policy.OnGet(obj);
            }
            catch
            {
                Return(obj);
                throw;
            }

            obj.LastGetThreadId = Thread.CurrentThread.ManagedThreadId;
            obj.LastGetTime = DateTime.Now;
            Interlocked.Increment(ref obj._getTimes);

            return obj;
        }

#if net40
#else
        public async Task<Object<T>> GetAsync()
        {
            var obj = GetFree(true);

            if (obj == null)
            {
                if (Policy.AsyncGetCapacity > 0 && _getAsyncQueue.Count >= Policy.AsyncGetCapacity - 1)
                    throw new OutOfMemoryException($"ObjectPool.GetAsync 无可用资源且队列过长，Policy.AsyncGetCapacity = {Policy.AsyncGetCapacity}。");

                var tcs = new TaskCompletionSource<Object<T>>();

                _getAsyncQueue.Enqueue(tcs);
                _getQueue.Enqueue(true);

                obj = await tcs.Task;

                //if (timeout == null) timeout = Policy.SyncGetTimeout;

                //if (tcs.Task.Wait(timeout.Value))
                //	obj = tcs.Task.Result;

                //if (obj == null) {

                //	tcs.TrySetCanceled();
                //	Policy.GetTimeout();

                //	if (Policy.IsThrowGetTimeoutException)
                //		throw new Exception($"ObjectPool.GetAsync 获取超时（{timeout.Value.TotalSeconds}秒）。");

                //	return null;
                //}
            }

            try
            {
                await Policy.OnGetAsync(obj);
            }
            catch
            {
                Return(obj);
                throw;
            }

            obj.LastGetThreadId = Thread.CurrentThread.ManagedThreadId;
            obj.LastGetTime = DateTime.Now;
            Interlocked.Increment(ref obj._getTimes);

            return obj;
        }
#endif

        public void Return(Object<T> obj, bool isReset = false)
        {

            if (obj == null) return;

            if (obj.IsReturned) return;

            if (_running == false)
            {

                Policy.OnDestroy(obj.Value);
                try { (obj.Value as IDisposable)?.Dispose(); } catch { }

                return;
            }

            if (isReset) obj.ResetValue();

            var isReturn = false;

            while (isReturn == false && _getQueue.TryDequeue(out var isAsync))
            {

                if (isAsync == false)
                {

                    if (_getSyncQueue.TryDequeue(out var queueItem) && queueItem != null)
                    {

                        lock (queueItem.Lock)
                            if (queueItem.IsTimeout == false)
                                queueItem.ReturnValue = obj;

                        if (queueItem.ReturnValue != null)
                        {

                            obj.LastReturnThreadId = Thread.CurrentThread.ManagedThreadId;
                            obj.LastReturnTime = DateTime.Now;

                            try
                            {
                                queueItem.Wait.Set();
                                isReturn = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }

                        try { queueItem.Dispose(); }
                        catch
                        {
                            // ignored
                        }
                    }

                }
                else
                {

                    if (_getAsyncQueue.TryDequeue(out var tcs) && tcs != null && tcs.Task.IsCanceled == false)
                    {

                        obj.LastReturnThreadId = Thread.CurrentThread.ManagedThreadId;
                        obj.LastReturnTime = DateTime.Now;

                        try { isReturn = tcs.TrySetResult(obj); } catch { }
                    }
                }
            }

            //无排队，直接归还
            if (isReturn == false)
            {
                try
                {
                    Policy.OnReturn(obj);
                }
                finally
                {
                    obj.LastReturnThreadId = Thread.CurrentThread.ManagedThreadId;
                    obj.LastReturnTime = DateTime.Now;
                    obj.IsReturned = true;

                    _freeObjects.Push(obj);
                }
            }
        }

        public void Dispose()
        {
            _running = false;

            while (_freeObjects.TryPop(out var fo)) ;

            while (_getSyncQueue.TryDequeue(out var sync))
            {
                try { sync.Wait.Set(); }
                catch
                {
                    // ignored
                }
            }

            while (_getAsyncQueue.TryDequeue(out var async))
                async.TrySetCanceled();

            while (_getQueue.TryDequeue(out var qs)) ;

            foreach (var t in _allObjects)
            {
                Policy.OnDestroy(t.Value);
                try { (t.Value as IDisposable)?.Dispose(); }
                catch
                {
                    // ignored
                }
            }

            _allObjects.Clear();
        }

        class GetSyncQueueInfo : IDisposable
        {
            internal ManualResetEventSlim Wait { get; set; } = new ManualResetEventSlim();

            internal Object<T> ReturnValue { get; set; }

            internal readonly object Lock = new object();

            internal bool IsTimeout { get; set; } = false;

            public void Dispose()
            {
                try
                {
                    Wait?.Dispose();
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
