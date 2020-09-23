using System;

namespace MDA.Shared.Utils
{
    public class PeriodicAction
    {
        private readonly Action _action;
        private readonly TimeSpan _period;
        private DateTime _nextUtc;

        public PeriodicAction(TimeSpan period, Action action, DateTime? start = null)
        {
            _period = period;
            _nextUtc = start ?? DateTime.UtcNow + period;
            _action = action;
        }

        public bool TryAction(DateTime nowUtc)
        {
            if (nowUtc < _nextUtc) return false;
            _nextUtc = nowUtc + _period;
            _action();
            return true;
        }
    }
}
