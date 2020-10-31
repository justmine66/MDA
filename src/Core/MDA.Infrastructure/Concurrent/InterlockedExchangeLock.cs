using System.Threading;

namespace MDA.Infrastructure.Concurrent
{
    public class InterlockedExchangeLock
    {
        private const int Locked = 1;
        private const int Unlocked = 0;
        private int _lockState = Unlocked;

        public bool TryGetLock()
        {
            return Interlocked.Exchange(ref _lockState, Locked) != Locked;
        }

        public void ReleaseLock()
        {
            Interlocked.Exchange(ref _lockState, Unlocked);
        }
    }
}
