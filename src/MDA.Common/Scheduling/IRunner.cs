using System;

namespace MDA.Common.Scheduling
{
    /// <summary>
    /// 以固定的间隔不断地执行任务的运行器。
    /// </summary>
    public interface IRunner
    {
        void Run(TimeSpan interval, Action callback);
        void Run<TState>(TimeSpan interval, Action<TState> callback, TState state);
    }
}
