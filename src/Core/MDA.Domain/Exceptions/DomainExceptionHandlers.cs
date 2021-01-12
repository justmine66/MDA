using MDA.MessageBus;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Exceptions
{
    public interface IDomainExceptionHandler<in TDomainException> : IMessageHandler<TDomainException>
        where TDomainException : IDomainExceptionMessage
    {
        new void Handle(TDomainException exception);
    }

    public interface IAsyncDomainExceptionHandler<in TDomainException> : IAsyncMessageHandler<TDomainException>
        where TDomainException : IDomainExceptionMessage
    {
        new Task HandleAsync(TDomainException exception, CancellationToken token = default);
    }
}
