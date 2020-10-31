using Microsoft.Extensions.DependencyInjection;

namespace MDA.MessageBus.DependencyInjection
{
    public class DefaultMessageBusConfigureContext : IMessageBusConfigureContext
    {
        public DefaultMessageBusConfigureContext(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
