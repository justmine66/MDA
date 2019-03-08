using System.Threading.Tasks;

namespace MDA.Common.Concurrent
{
    public interface ICancellable
    {
        void Cancel();
    }

    public interface IAsyncCancellable : ICancellable
    {
        Task CancelAsync();
    }

    public interface ICancellable<in TState> where TState : class
    {
        void Cancel(TState state);
    }

    public interface IAsyncCancellable<in TState> : ICancellable<TState> where TState : class
    {
        Task CancelAsync(TState state);
    }
}
