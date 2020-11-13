using Microsoft.Extensions.DependencyInjection;

namespace MDA.Infrastructure.Serialization
{
    internal static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddSerialization(this IServiceCollection services)
        {
            services.AddSingleton<IJsonSerializer, NewtonsoftJsonSerializer>();
            services.AddSingleton<IBinarySerializer, DefaultBinarySerializer>();

            return services;
        }
    }
}
