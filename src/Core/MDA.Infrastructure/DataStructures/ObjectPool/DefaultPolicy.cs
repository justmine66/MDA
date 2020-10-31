using System;
using System.Threading.Tasks;

namespace MDA.Infrastructure.DataStructures.ObjectPool
{
    public class DefaultPolicy<T> : IPolicy<T>
    {
        public string Name { get; set; } = typeof(DefaultPolicy<T>).FullName;
        public int PoolSize { get; set; } = 1000;
        public TimeSpan SyncGetTimeout { get; set; } = TimeSpan.FromSeconds(10);
        public TimeSpan IdleTimeout { get; set; } = TimeSpan.FromSeconds(50);
        public int AsyncGetCapacity { get; set; } = 10000;
        public bool IsThrowGetTimeoutException { get; set; } = true;
        public bool IsAutoDisposeWithSystem { get; set; } = true;
        public int CheckAvailableInterval { get; set; } = 5;

        public Func<T> CreateObject;
        public Action<Object<T>> OnGetObject;

        public T OnCreate() => CreateObject();

        public void OnDestroy(T obj) { }

        public void OnGetTimeout() { }

        public void OnGet(Object<T> obj) => OnGetObject?.Invoke(obj);
#if net40
#else
        public Task OnGetAsync(Object<T> obj)
        {
            OnGetObject?.Invoke(obj);
            return Task.FromResult(true);
        }
#endif
        public void OnReturn(Object<T> obj) { }

        public bool OnCheckAvailable(Object<T> obj) => true;

        public void OnAvailable() { }

        public void OnUnavailable() { }
    }
}
