using MDA.MessageBus;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MDA.Tests.Messaging
{
    public class TestDynamicMessageHandler : IDynamicMessageHandler
    {
        public static bool ReceivedMessage = false;
        public async Task HandleAsync(dynamic message)
        {
            Debugger.Log(1, "TestMessage", message.ToString());

            ReceivedMessage = true;

            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
}
