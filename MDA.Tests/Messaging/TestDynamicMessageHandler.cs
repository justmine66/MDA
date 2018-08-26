using System.Diagnostics;
using System.Threading.Tasks;
using MDA.Messaging;

namespace MDA.Tests.Messaging
{
    public class TestDynamicMessageHandler : IDynamicMessageHandler
    {
        public static bool ReceivedMessage = false;
        public Task HandleAsync(dynamic message)
        {
            Debugger.Log(1, "TestMessage", message.ToString());

            ReceivedMessage = true;

            return Task.CompletedTask;
        }
    }
}
