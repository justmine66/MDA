using System.Threading;
using MDA.Disruptor.Infrastracture;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// 表示ring buffer的序列号。
    /// </summary>
    /// <remarks>通过增加补全来确保ring buffer的序列号不会和其他东西同时存在于一个缓存行中。没有伪共享，就没有和其它任何变量的意外冲突，没有不必要的缓存未命中。</remarks>
    public class Sequence : RhsPadding, ISequence
    {
        /// <summary>
        /// Set to -1 as sequence starting point.
        /// </summary>
        public const long InitialValue = -1L;

        /// <summary>
        /// Create a sequence initialised to -1.
        /// </summary>
        public Sequence() : this(InitialValue) { }

        /// <summary>
        /// Create a sequence with a specified initial value.
        /// </summary>
        /// <param name="initialValue">The initial value for this sequence.</param>
        public Sequence(long initialValue = InitialValue)
        {
            Value = initialValue;
        }

        public virtual long GetValue()
        {
            return Volatile.Read(ref Value);
        }

        public virtual void SetValue(long value)
        {
            Value = value;
        }

        public virtual void SetVolatileValue(long value)
        {
            Volatile.Write(ref Value, value);
        }

        public virtual bool CompareAndSet(long expectedValue, long newValue)
        {
            return Interlocked.CompareExchange(ref Value, newValue, expectedValue) == expectedValue;
        }

        public virtual long IncrementAndGet()
        {
            return Interlocked.Increment(ref Value);
        }

        public virtual long AddAndGet(long increment)
        {
            return Interlocked.Add(ref Value, increment);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
