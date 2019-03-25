using Disruptor.Dsl;
using MDA.Concurrent;
using MDA.Eventing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MDA
{
    public static class ServiceCollectionExtension
    {
        public static void AddMdaServices(this IServiceCollection container, IConfiguration configuration)
        {
            container.Configure<DisruptorOptions>(configuration.GetSection(nameof(DisruptorOptions)));
        }

        private static IServiceCollection AddMDABasicServices(this IServiceCollection services)
        {
            services.AddOptions();
            services.AddSingleton<Disruptor<InboundEvent>>(new Disruptor<InboundEvent>());
            return services;
        }
    }
}
