namespace MDA.Common.Concurrent
{
    public interface IRunnable
    {
        void Run();
    }

    public interface IRunnable<in TState>
    {
        void Run(TState state);
    }
}
