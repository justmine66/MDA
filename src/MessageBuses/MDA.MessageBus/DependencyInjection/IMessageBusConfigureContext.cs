using Microsoft.Extensions.DependencyInjection;

namespace MDA.MessageBus.DependencyInjection
{
    public interface IMessageBusConfigureContext
    {
        IServiceCollection Services { get; }
    }
}
