using MDA.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
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
