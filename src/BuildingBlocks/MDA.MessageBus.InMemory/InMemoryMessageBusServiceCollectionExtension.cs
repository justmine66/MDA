using Microsoft.Extensions.DependencyInjection;

namespace MDA.MessageBus.InMemory
{
    public static class InMemoryMessageBusServiceCollectionExtension
    {
        public static IServiceCollection AddInMemoryMessageBusServices(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBus, InMemoryMessageBus>();
            return services;
        }
    }
}
