using MDA.Domain.Shared.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Exceptions
{
    public interface IDomainExceptionMessagePublisher
    {
        void Publish(IDomainExceptionMessage exception);

        Task PublishAsync(IDomainExceptionMessage exception, CancellationToken token = default);
    }
}
