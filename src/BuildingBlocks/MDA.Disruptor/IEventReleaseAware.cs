namespace MDA.Disruptor
{
    public interface IEventReleaseAware
    {
        void SetEventReleaser(IEventReleaser eventReleaser);
    }
}
