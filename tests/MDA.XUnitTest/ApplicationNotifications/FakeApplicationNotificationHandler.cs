using MDA.Application.Notifications;
using Microsoft.Extensions.Logging;

namespace MDA.XUnitTest.ApplicationNotifications
{
    public class FakeApplicationNotificationHandler : IApplicationNotificationHandler<FakeApplicationNotification>
    {
        private readonly ILogger _logger;

        public FakeApplicationNotificationHandler(ILogger<FakeApplicationNotificationHandler> logger)
        {
            _logger = logger;
        }

        public void Handle(FakeApplicationNotification notification)
        {
            _logger.LogInformation($"The application nootification: {nameof(notification)}[Payload: {notification.Payload}] handled.");
        }
    }
}
