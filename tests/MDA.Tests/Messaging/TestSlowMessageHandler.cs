using MDA.MessageBus;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace MDA.Tests.Messaging
{
    public class TestSlowMessageHandler : IMessageHandler<TestMessage>
    {
        private readonly MessageOptions _options;

        public TestSlowMessageHandler(IOptions<MessageOptions> options)
        {
            _options = options.Value;
        }

        public async Task HandleAsync(TestMessage message)
        {
            var handleTime = _options.SlowMessageHandlerThreshold.Add(TimeSpan.FromSeconds(1));
            await Task.Delay(handleTime);
        }
    }
}
