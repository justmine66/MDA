using System.Threading.Tasks;

namespace MDA.Common.Concurrent
{
    public interface IExecutable
    {
        ICancellable Execute(IRunnable command);
    }

    public interface IAsyncExecutable : IExecutable
    {
        Task<ICancellable> ExecuteAsync(IAsyncRunnable command);
    }

    public interface IExecutable<TState> where TState : class
    {
        ICancellable<TState> Execute(IRunnable<TState> state);
    }

    public interface IAsyncExecutable<TState> : IExecutable<TState> where TState : class
    {
        Task<ICancellable<TState>> ExecuteAsync(IAsyncRunnable<TState> state);
    }
}
