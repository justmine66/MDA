using Microsoft.Extensions.Primitives;
using System;
using System.Threading;

namespace MDA.Common.Scheduling
{
    public class Runner : IRunner
    {
        public void Run(TimeSpan interval, Action callback)
        {
            ChangeToken.OnChange(() => new CancellationChangeToken(new CancellationTokenSource(interval).Token), callback);
        }
        public void Run<TState>(TimeSpan interval, Action<TState> callback, TState state)
        {
            ChangeToken.OnChange(() => new CancellationChangeToken(new CancellationTokenSource(interval).Token), callback, state);
        }
    }
}
