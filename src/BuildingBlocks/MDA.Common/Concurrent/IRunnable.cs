using System.Threading.Tasks;

namespace MDA.Common.Concurrent
{
    public interface IRunnable
    {
        void Run();
    }

    public interface IAsyncRunnable : IRunnable
    {
        Task RunAsync();
    }

    public interface IRunnable<in TState> where TState : class
    {
        void Run(TState state);
    }

    public interface IAsyncRunnable<in TState> : IRunnable<TState> where TState : class
    {
        Task RunAsync(TState state);
    }
}
