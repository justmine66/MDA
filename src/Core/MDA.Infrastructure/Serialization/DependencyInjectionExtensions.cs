using Microsoft.Extensions.DependencyInjection;

namespace MDA.Infrastructure.Serialization
{
    internal static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddSerialization(this IServiceCollection services)
        {
            services.AddSingleton<IBinarySerializer, DefaultBinarySerializer>();
            services.AddSingleton<IJsonSerializer, DefaultJsonSerializer>();

            return services;
        }
    }
}
