using Disruptor;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace MDA.MessageBus.Disruptor
{
    public class DisruptorTransportEventHandler : IEventHandler<DisruptorTransportEvent>
    {
        public void OnEvent(DisruptorTransportEvent data, long sequence, bool endOfBatch)
        {
            using (var scope = data.ServiceProvider.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var proxies = serviceProvider.GetServices<IMessageHandlerProxy>();
                if (proxies != null)
                {
                    foreach (var proxy in proxies)
                    {
                        proxy?.Handle(data.Message);
                    }
                }

                var asyncProxies = serviceProvider.GetServices<IAsyncMessageHandlerProxy>();
                if (asyncProxies != null)
                {
                    foreach (var proxy in asyncProxies)
                    {
                        proxy?.HandleAsync(data.Message, CancellationToken.None).GetAwaiter().GetResult();
                    }
                }
            }
        }
    }
}
