using Microsoft.Extensions.DependencyInjection;
using System;

namespace MDA.Common.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServices<TServiceType, TImplementationType>(this IServiceCollection services, ServiceLifetime lifetime)
        {
            return services.AddServices(typeof(TServiceType), typeof(TImplementationType), lifetime);
        }

        public static IServiceCollection AddServices(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (serviceType == (Type)null)
                throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == (Type)null)
                throw new ArgumentNullException(nameof(implementationType));

            var serviceDescriptor = new ServiceDescriptor(serviceType, implementationType, lifetime);
            services.Add(serviceDescriptor);

            return services;
        }
    }
}
