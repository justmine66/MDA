using Microsoft.Extensions.DependencyInjection;

namespace MDA.Shared.Serialization
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddSerialization(this IServiceCollection services)
        {
            services.AddSingleton<IBinarySerializer, DefaultBinarySerializer>();
            services.AddSingleton<IJsonSerializer, DefaultJsonSerializer>();

            return services;
        }
    }
}
