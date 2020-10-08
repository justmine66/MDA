using Microsoft.Extensions.DependencyInjection;

namespace MDA.Shared.Types
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddTypes(this IServiceCollection services)
        {
            services.AddSingleton<ITypeResolver, CachedTypeResolver>();

            return services;
        }
    }
}
