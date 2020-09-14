using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MDA.MessageBus.Disruptor
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddMessageBusDisruptor(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddMessageBusCore(assemblies);
            services.AddSingleton<IMessageQueueService, DisruptorMessageQueueService>();

            return services;
        }
    }
}
