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
    /// 启动(1-CPU核数)个线程，每个线程独立更新自己的计数器，测试持续时间与线程数的线性关系。
    /// </remarks>
    public sealed class FalseSharingTest
    {
        public const int NumThreads = 4;
        public const long Iterations = 500L * 1000L * 1000L;//迭代500亿次。

        //private readonly FalseSharingCacheLineEntry[] _seqs;
        private readonly CacheLineEntry[] _seqs;
        //private readonly CacheLineEntryOne[] _seqs;

        public FalseSharingTest()
        {
            //_seqs = new FalseSharingCacheLineEntry[NumThreads];
            _seqs = new CacheLineEntry[NumThreads];
            //_seqs = new CacheLineEntryOne[NumThreads];
            for (int i = 0; i < _seqs.Length; i++)
            {
                //_seqs[i] = new FalseSharingCacheLineEntry();
                _seqs[i] = new CacheLineEntry();
                //_seqs[i] = new CacheLineEntryOne();
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
            var threads = new Thread[NumThreads];
            for (var i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(SetValue);
            }

            for (var i = 0; i < threads.Length; i++)
            {
                threads[i].Start(i);
            }

            foreach (var t in threads)
            {
                t.Join();
            }
        }

        private void SetValue(object obj)
        {
            if (int.TryParse(obj?.ToString(), out int index))
            {
                var iterations = Iterations;
                while (0 != --iterations)
                {
                    _seqs[index].Value = iterations;
                }
            }
        }
    }

    ///// <summary>
    ///// CPU伪共享高速缓存行条目(伪共享)
    ///// </summary>
    public class FalseSharingCacheLineEntry
    {
        public long Value = 0L;
    }

    /// <summary>
    /// CPU高速缓存行条目(直接填充)
    /// </summary>
    public class CacheLineEntry
    {
        protected long P1, P2, P3, P4, P5, P6, P7;
        public long Value = 0L;
        protected long P9, P10, P11, P12, P13, P14, P15;
    }

    /// <summary>
    /// CPU高速缓存行条目(控制内存布局)
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 120)]
    public class CacheLineEntryOne
    {
        [FieldOffset(56)]
        private long _value;

        public long Value
        {
            get => _value;
            set => _value = value;
        }
    }
}
