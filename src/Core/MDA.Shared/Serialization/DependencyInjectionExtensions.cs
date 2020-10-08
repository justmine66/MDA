using Microsoft.Extensions.DependencyInjection;

namespace MDA.Shared.Serialization
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddSerialization(this IServiceCollection services)
        {
            services.AddSingleton<IBinarySerializer, MessagePackBinarySerializer>();

            return services;
        }
    }
}
