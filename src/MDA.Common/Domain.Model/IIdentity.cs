namespace MDA.Common.Domain.Model
{
    public interface IIdentity<TType>
    {
        TType Id { get;  }
    }
}
