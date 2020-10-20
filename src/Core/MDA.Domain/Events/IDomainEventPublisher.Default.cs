using MDA.MessageBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Events
{
    public class DefaultDomainEventPublisher : IDomainEventPublisher
    {
        private readonly IMessagePublisher _messagePublisher;

        public DefaultDomainEventPublisher(IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        public void Publish(IDomainEvent @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            _messagePublisher.Publish(@event);
        }

        public void Publish<TAggregateRootId>(IDomainEvent<TAggregateRootId> @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            _messagePublisher.Publish(@event);
        }

        public async Task PublishAsync(IDomainEvent @event, CancellationToken token)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            await _messagePublisher.PublishAsync(@event, token);
        }

        public async Task PublishAsync<TAggregateRootId>(IDomainEvent<TAggregateRootId> @event, CancellationToken token)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            await _messagePublisher.PublishAsync(@event, token);
        }
    }
}
