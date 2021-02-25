using MDA.Domain.Shared.Notifications;
using MDA.MessageBus;
using MDA.MessageBus.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Notifications
{
    [IgnoreMessageHandlerForDependencyInjection]
    public class DomainNotificationProcessor<TDomainNotification> :
        IMessageHandler<TDomainNotification>,
        IAsyncMessageHandler<TDomainNotification>
        where TDomainNotification : class, IDomainNotification
    {
        private readonly ILogger _logger;
        private readonly IDomainNotifyingContext _context;

        public DomainNotificationProcessor(
            IDomainNotifyingContext context,
            ILogger<DomainNotificationProcessor<TDomainNotification>> logger)
        {
            _logger = logger;
            _context = context;
        }

        public void Handle(TDomainNotification notification)
        {
            using var scope = _context.ServiceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetService<IDomainNotificationHandler<TDomainNotification>>();
            if (handler == null)
            {
                _logger.LogError($"The {typeof(IDomainNotificationHandler<TDomainNotification>).FullName} no found.");

                return;
            }

            _context.SetDomainNotification(notification);

            handler.OnDomainNotification(_context, notification);
        }

        public async Task HandleAsync(TDomainNotification notification, CancellationToken token = default)
        {
            using var scope = _context.ServiceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetService<IAsyncDomainNotificationHandler<TDomainNotification>>();
            if (handler == null)
            {
                _logger.LogError($"The {typeof(IAsyncDomainNotificationHandler<TDomainNotification>).FullName} no found.");

                return;
            }

            _context.SetDomainNotification(notification);

            await handler.OnDomainNotificationAsync(_context, notification, token);
        }
    }
}
