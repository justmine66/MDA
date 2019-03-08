using MDA.MessageBus;
using System.Threading.Tasks;

namespace MDA.Tests.Messaging
{
    public class TestDynamicMessageHandler1 : IDynamicMessageHandler
    {
        public Task HandleAsync(dynamic message)
        {
            throw new System.NotImplementedException();
        }
    }
}
