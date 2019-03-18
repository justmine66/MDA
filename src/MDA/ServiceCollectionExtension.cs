using MDA.Concurrent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MDA
{
    public static class ServiceCollectionExtension
    {
        public static void AddMdaServices(this IServiceCollection container, IConfiguration configuration)
        {
            container.AddOptions<DisruptorOptions>(configuration.GetSection(nameof(DisruptorOptions)));
        }

        private static IServiceCollection AddMDABasicServices(this IServiceCollection services)
        {
            services.AddOptions();
            services.AddSingleton<IInboundDisruptor<>>();
            return services;
        }
    }
}
