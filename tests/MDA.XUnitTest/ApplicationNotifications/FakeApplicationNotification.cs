using MDA.Application.Notifications;

namespace MDA.XUnitTest.ApplicationNotifications
{
    public class FakeApplicationNotification : ApplicationNotification
    {
        public long Payload { get; set; }
    }
}
