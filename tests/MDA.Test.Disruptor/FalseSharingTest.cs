using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace MDA.Test.Disruptor
{
    /// <summary>
    /// CPU高速缓存伪共享测试类。
    /// </summary>
    /// <remarks>
    /// 启动CPU核数个线程，每个线程独立更新自己的计数器。
    /// </remarks>
    public sealed class FalseSharingTest
    {
        public const int NUM_THREADS = 4;
        public const long ITERATIONS = 500L * 1000L * 1000L;//迭代500亿次。

        private readonly CacheLineEntry[] longs = new CacheLineEntry[NUM_THREADS];

        public FalseSharingTest()
        {
            for (int i = 0; i < longs.Length; i++)
            {
                longs[i] = new CacheLineEntry();
            }
        }

        public void StartTest()
        {
            var watch = new Stopwatch();
            watch.Start();
            Console.WriteLine($"test start...");
            Run();
            Console.WriteLine($"test end, durationInMilliseconds: {watch.Elapsed.TotalMilliseconds}");
            watch.Stop();
        }

        private void Run()
        {
            var threads = new Thread[NUM_THREADS];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(SetValue);
            }

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Start(i);
            }

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }
        }

        private void SetValue(object obj)
        {
            if (int.TryParse(obj?.ToString(), out int index))
            {
                var iterations = ITERATIONS;
                while (0 != --iterations)
                {
                    longs[index].Value = iterations;
                }
            }
        }
    }

    /// <summary>
    /// CPU高速缓存行条目
    /// </summary>
    public class CacheLineEntry
    {
        protected long p1, p2, p3, p4, p5, p6, p7;
        public long Value = 0L;
        protected long p9, p10, p11, p12, p13, p14, p15;
    }

    //[StructLayout(LayoutKind.Explicit, Size = 128)]
    //public class CacheLineEntryOne
    //{
    //    [FieldOffset(56)]
    //    private long _value;

    //    public long Value
    //    {
    //        get => _value;
    //        set => _value = value;
    //    }
    //}
}
