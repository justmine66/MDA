namespace MDA.Common.Domain.Model
{
    public interface IIdentity<out TType>
    {
        TType Id { get;  }
    }
}
