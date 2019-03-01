namespace MDA.Disruptor
{
    public interface ITimeoutHandler
    {
        void OnTimeout(long sequence);
    }
}
