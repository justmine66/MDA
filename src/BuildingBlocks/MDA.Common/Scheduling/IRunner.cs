using System;

namespace MDA.Common.Scheduling
{
    /// <summary>
    /// 以固定时间间隔循环执行任务的运行器。
    /// </summary>
    public interface IRunner
    {
        void Run(TimeSpan interval, Action callback);
        void Run<TState>(TimeSpan interval, Action<TState> callback, TState state);
    }
}
