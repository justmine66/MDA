using MDA.MessageBus;
using System;

namespace MDA.Domain.Commands
{
    public class DefaultDomainCommandPublisher : IDomainCommandPublisher
    {
        private readonly IMessagePublisher _messagePublisher;

        public DefaultDomainCommandPublisher(IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        public void Publish(IDomainCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var message = new DomainCommandTransportMessage(command);

            _messagePublisher.Publish(message);
        }

        public void Publish<TAggregateRootId>(IDomainCommand<TAggregateRootId> command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var message = new DomainCommandTransportMessage<TAggregateRootId>(command);

            _messagePublisher.Publish(message);
        }
    }
}
