using System.Collections.Concurrent;

namespace MDA.Infrastructure.Concurrent
{
    public class CompatibleConcurrentQueue<T> : ConcurrentQueue<T>, IQueue<T>
    {
        public bool TryEnqueue(T element)
        {
            Enqueue(element);

            return true;
        }

        void IQueue<T>.Clear()
        {
            while (TryDequeue(out _)) { }
        }
    }
}
