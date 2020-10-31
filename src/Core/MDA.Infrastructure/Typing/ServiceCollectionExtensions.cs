using Microsoft.Extensions.DependencyInjection;

namespace MDA.Infrastructure.Typing
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTyping(this IServiceCollection services)
        {
            services.AddSingleton<ITypeResolver, CachedTypeResolver>();

            return services;
        }
    }
}
