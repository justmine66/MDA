namespace MDA.EventSourcing
{
    public interface IIdentity<out T>
    {
        T Id { get; }
    }
}
