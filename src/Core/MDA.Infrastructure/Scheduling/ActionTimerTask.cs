using System;

namespace MDA.Infrastructure.Scheduling
{
    public class ActionTimerTask : ITimerTask
    {
        private readonly Action<ITimeout> _action;

        public ActionTimerTask(Action<ITimeout> action) => _action = action;

        public void Run(ITimeout timeout) => _action(timeout);
    }
}
