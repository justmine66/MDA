using Microsoft.Extensions.DependencyInjection;

namespace MDA.Infrastructure.Scheduling
{
    internal static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddScheduling(this IServiceCollection services)
        {
            services.AddSingleton<ITimer, HashedWheelTimer>();

            return services;
        }
    }
}
