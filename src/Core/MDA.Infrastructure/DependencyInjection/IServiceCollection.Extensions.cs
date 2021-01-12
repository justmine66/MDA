using Microsoft.Extensions.DependencyInjection;
using System;

namespace MDA.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMda(
            this IServiceCollection services,
            Action<IMdaConfigureContext> configure)
        {
            configure(new DefaultMdaConfigureContext(services));

            return services;
        }
    }
}
