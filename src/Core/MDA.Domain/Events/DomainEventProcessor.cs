using MDA.Domain.Events;
using MDA.Domain.Notifications;
using MDA.Domain.Saga;
using MDA.MessageBus;
using MDA.MessageBus.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Events
{
    [IgnoreMessageHandlerForDependencyInjection]
    public class DomainEventProcessor<TDomainEvent> :
        IMessageHandler<TDomainEvent>,
        IAsyncMessageHandler<TDomainEvent>
        where TDomainEvent : class, IDomainEvent
    {
        private readonly ILogger _logger;
        private readonly IDomainEventingContext _context;

        public DomainEventProcessor(
            IDomainEventingContext context,
            ILogger<DomainEventProcessor<TDomainEvent>> logger)
        {
            _logger = logger;
            _context = context;
        }

        public void Handle(TDomainEvent @event)
        {
            using var scope = _context.ServiceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetService<IDomainEventHandler<TDomainEvent>>();
            if (handler == null)
            {
                _logger.LogError($"The {typeof(IDomainEventHandler<TDomainEvent>).FullName} no found.");

                return;
            }

            _context.SetDomainEvent(@event);

            handler.OnDomainEvent(_context, @event);

            if (!@event.NeedReplyApplicationCommand()) return;

            ReplyApplicationEvent(@event);
        }

        public async Task HandleAsync(TDomainEvent @event, CancellationToken token = default)
        {
            using var scope = _context.ServiceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetService<IAsyncDomainEventHandler<TDomainEvent>>();
            if (handler == null)
            {
                _logger.LogError($"The {typeof(IAsyncDomainEventHandler<TDomainEvent>).FullName} no found.");

                return;
            }

            _context.SetDomainEvent(@event);

            await handler.OnDomainEventAsync(_context, @event, token);

            if (!@event.NeedReplyApplicationCommand()) return;

            await ReplyApplicationEventAsync(@event, token);
        }

        private void ReplyApplicationEvent(IDomainEvent @event)
        {
            IDomainNotification notification;

            switch (@event)
            {
                case IEndSubTransactionDomainEvent endEvent:
                    notification = new SagaTransactionDomainNotification(endEvent.Message, true);
                    break;
                default:
                    notification = new DomainEventHandledNotification();
                    break;
            }

            notification.FillFrom(@event);

            _context.PublishDomainNotification(notification);
        }

        private async Task ReplyApplicationEventAsync(IDomainEvent @event, CancellationToken token)
        {
            IDomainNotification notification;

            switch (@event)
            {
                case IEndSubTransactionDomainEvent endEvent:
                    notification = new SagaTransactionDomainNotification(endEvent.Message, true);
                    break;
                default:
                    notification = new DomainEventHandledNotification();
                    break;
            }

            notification.FillFrom(@event);

            await _context.PublishDomainNotificationAsync(notification, token);
        }
    }
}
