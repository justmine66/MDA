using MDA.Messaging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MDA.Tests.Messaging
{
    public class TestMessageHandler : IMessageHandler<TestMessage>
    {
        public static bool ReceivedMessage = false;

        public Task HandleAsync(TestMessage message)
        {
            Debugger.Log(1, "TestMessage", message.ToString());

            ReceivedMessage = true;

            return Task.CompletedTask;
        }
    }
}
