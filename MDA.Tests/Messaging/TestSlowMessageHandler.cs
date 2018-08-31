using MDA.Message;
using MDA.Message.Abstractions;
using System;
using System.Threading.Tasks;

namespace MDA.Tests.Messaging
{
    public class TestSlowMessageHandler : IMessageHandler<TestMessage>
    {
        private readonly MessageOptions _options;

        public TestSlowMessageHandler(MessageOptions options)
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
