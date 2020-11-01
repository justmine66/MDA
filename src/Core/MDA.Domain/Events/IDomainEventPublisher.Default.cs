using MDA.MessageBus;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace MDA.Domain.Events
{
    public class DefaultDomainEventPublisher : IDomainEventPublisher
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly DomainEventOptions _options;

        public DefaultDomainEventPublisher(
            IMessagePublisher messagePublisher, 
            IOptions<DomainEventOptions> options)
        {
            _messagePublisher = messagePublisher;
            _options = options.Value;
        }

        public void Publish(IDomainEvent @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            @event.Topic = _options.Topic;

            _messagePublisher.Publish(@event);
        }

        public void Publish<TAggregateRootId>(IDomainEvent<TAggregateRootId> @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            @event.Topic = _options.Topic;

            _messagePublisher.Publish(@event);
        }

        public async Task PublishAsync(IDomainEvent @event, CancellationToken token)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            @event.Topic = _options.Topic;

            await _messagePublisher.PublishAsync(@event, token);
        }

        public async Task PublishAsync<TAggregateRootId>(IDomainEvent<TAggregateRootId> @event, CancellationToken token)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            @event.Topic = _options.Topic;

            await _messagePublisher.PublishAsync(@event, token);
        }
    }
}
