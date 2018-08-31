using MDA.Message.Abstractions;
using System;
using System.Threading.Tasks;

namespace MDA.Tests.Messaging
{
    public class TestMessageHandler1 : IMessageHandler<TestMessage>
    {
        public Task HandleAsync(TestMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
