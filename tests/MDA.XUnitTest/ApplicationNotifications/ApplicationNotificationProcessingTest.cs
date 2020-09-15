using MDA.Application.Notifications;
using Xunit;

namespace MDA.XUnitTest.ApplicationNotifications
{
    public class ApplicationNotificationProcessingTest
    {
        private readonly IApplicationNotificationPublisher _publisher;

        public ApplicationNotificationProcessingTest(IApplicationNotificationPublisher publisher)
        {
            _publisher = publisher;
        }

        [Fact(DisplayName = "测试应用通知消息")]
        public void TestProcessMessage()
        {
            var notification = new FakeApplicationNotification() { Payload = 1 };

            _publisher.Publish(notification);
        }
    }
}
