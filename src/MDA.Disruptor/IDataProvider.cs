namespace MDA.Disruptor
{
    public interface IDataProvider<T>
    {
        T Get(long sequence);
    }
}