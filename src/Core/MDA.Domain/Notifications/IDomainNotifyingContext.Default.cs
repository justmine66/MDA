using MDA.Domain.Commands;
using MDA.Domain.Exceptions;
using MDA.Domain.Shared.Commands;
using MDA.Domain.Shared.Exceptions;
using MDA.Domain.Shared.Notifications;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Notifications
{
    public class DefaultDomainNotifyingContext : IDomainNotifyingContext
    {
        public IServiceProvider ServiceProvider { get; }

        private readonly IDomainCommandPublisher _domainCommandPublisher;
        private readonly IDomainExceptionMessagePublisher _domainExceptionMessagePublisher;

        private IDomainNotification _domainNotification;

        public DefaultDomainNotifyingContext(
            IServiceProvider serviceProvider,
            IDomainCommandPublisher domainCommandPublisher,
            IDomainExceptionMessagePublisher domainExceptionMessagePublisher)
        {
            ServiceProvider = serviceProvider;
            _domainCommandPublisher = domainCommandPublisher;
            _domainExceptionMessagePublisher = domainExceptionMessagePublisher;
        }

        public void SetDomainNotification(IDomainNotification domainNotification)
        {
            _domainNotification = domainNotification;
        }

        public void PublishDomainCommand(IDomainCommand command)
        {
            command.WithNotifyingContext(_domainNotification);

            _domainCommandPublisher.Publish(command);
        }

        public void PublishDomainCommand<TAggregateRootId>(IDomainCommand<TAggregateRootId> command)
        {
            command.WithNotifyingContext(_domainNotification);

            _domainCommandPublisher.Publish(command);
        }

        public void PublishDomainExceptionMessage(IDomainExceptionMessage exception)
        {
            exception.WithNotifyingContext(_domainNotification);

            _domainExceptionMessagePublisher.Publish(exception);
        }

        public async Task PublishDomainExceptionMessageAsync(IDomainExceptionMessage exception, CancellationToken token = default)
        {
            exception.WithNotifyingContext(_domainNotification);

            await _domainExceptionMessagePublisher.PublishAsync(exception, token);
        }
    }
}