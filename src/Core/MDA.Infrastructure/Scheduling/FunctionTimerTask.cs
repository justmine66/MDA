using System;
using System.Threading.Tasks;

namespace MDA.Infrastructure.Scheduling
{
    public class FunctionTimerTask : ITimerTask
    {
        private readonly Func<ITimeout, Task> _func;

        public FunctionTimerTask(Func<ITimeout, Task> function) => _func = function;

        public async void Run(ITimeout timeout) => await _func(timeout);
    }
}
