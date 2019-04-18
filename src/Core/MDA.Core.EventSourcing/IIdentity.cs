namespace MDA.Core.EventSourcing
{
    public interface IIdentity<out T>
    {
        T Id { get; }
    }
}
