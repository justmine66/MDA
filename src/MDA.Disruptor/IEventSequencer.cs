namespace MDA.Disruptor
{
    public interface IEventSequencer<TEvent> : IDataProvider<TEvent>, ISequenced
    {

    }
}
