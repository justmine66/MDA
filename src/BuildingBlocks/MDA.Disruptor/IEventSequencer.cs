namespace MDA.Disruptor
{
    public interface IEventSequencer<out TEvent> : IDataProvider<TEvent>, ISequenced
    {

    }
}
