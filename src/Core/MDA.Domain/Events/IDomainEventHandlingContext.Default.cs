using MDA.Domain.Commands;
using MDA.Domain.Exceptions;
using MDA.Domain.Notifications;
using System;

namespace MDA.Domain.Events
{
    public class DefaultDomainEventHandlingContext : IDomainEventHandlingContext
    {
        public DefaultDomainEventHandlingContext(
            IServiceProvider serviceProvider, 
            IDomainCommandPublisher domainCommandPublisher, 
            IDomainNotificationPublisher domainNotificationPublisher, 
            IDomainExceptionMessagePublisher domainExceptionMessagePublisher)
        {
            ServiceProvider = serviceProvider;
            DomainCommandPublisher = domainCommandPublisher;
            DomainNotificationPublisher = domainNotificationPublisher;
            DomainExceptionMessagePublisher = domainExceptionMessagePublisher;
        }

        public IServiceProvider ServiceProvider { get; }

        public IDomainCommandPublisher DomainCommandPublisher { get; }

        public IDomainNotificationPublisher DomainNotificationPublisher { get; }

        public IDomainExceptionMessagePublisher DomainExceptionMessagePublisher { get; }
    }
}
