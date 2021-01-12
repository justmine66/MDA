using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Exceptions
{
    public interface IDomainExceptionPublisher
    {
        void Publish(IDomainExceptionMessage exception);

        Task PublishAsync(IDomainExceptionMessage exception, CancellationToken token = default);
    }
}
