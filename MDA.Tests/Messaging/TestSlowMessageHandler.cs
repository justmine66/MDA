using MDA.Messaging;
using System;
using System.Threading.Tasks;

namespace MDA.Tests.Messaging
{
    public class TestSlowMessageHandler : IMessageHandler<TestMessage>
    {
        private readonly MessagingOptions _options;

        public TestSlowMessageHandler(MessagingOptions options)
        {
            _options = options;
        }

        public async Task HandleAsync(TestMessage message)
        {
            var handleTime = _options.SlowMessageHandlerThreshold.Add(TimeSpan.FromSeconds(1));
            await Task.Delay(handleTime);
        }
    }
}
