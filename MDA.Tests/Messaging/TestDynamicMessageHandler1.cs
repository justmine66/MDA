using System.Threading.Tasks;
using MDA.Messaging;

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
