namespace MDA.Domain.Model
{
    public interface IIdentity<TKey>
    {
        TKey Id { get; }
    }
}
