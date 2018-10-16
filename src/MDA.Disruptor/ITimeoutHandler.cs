namespace MDA.Disruptor.Impl
{
    public interface ITimeoutHandler
    {
        void OnTimeout(long sequence);
    }
}
