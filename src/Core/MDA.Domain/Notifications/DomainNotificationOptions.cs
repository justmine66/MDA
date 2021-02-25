using MDA.Domain.Shared;

namespace MDA.Domain.Notifications
{
    public class DomainNotificationOptions
    {
        public string Topic { get; set; } = DomainDefaults.Topics.Notification;
    }
}