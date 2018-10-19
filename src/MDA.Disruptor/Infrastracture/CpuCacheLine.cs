namespace MDA.Disruptor.Infrastracture
{
    public class LhsPadding
    {
        protected long P1, P2, P3, P4, P5, P6, P7;
    }

    /// <summary>
    /// CPU高速缓存系统中是以缓存行（cache line）为基本单位存储的。缓存行是2的整数幂个连续字节，一般为32-256个字节,一般为64个字节。当多线程修改互相独立的变量时，如果这些变量共享同一个缓存行，就会无意中影响彼此的性能，这就是伪共享（false sharing）。为了让可伸缩性与线程数呈线性关系，就必须确保不会有两个线程往同一个变量或缓存行中写。
    /// </summary>
    /// <remarks>缓存行大小是64个字节，一个long占据8 byte，在其前后填充7个long，这样无论Value在缓存行中的哪个位置都能保证始终独占整个缓存行。</remarks>
    public class CpuCacheLineValue : LhsPadding
    {
        protected long Value;
    }

    public class RhsPadding : CpuCacheLineValue
    {
        protected long P9, P10, P11, P12, P13, P14, P15;
    }
}
