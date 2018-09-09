using MDA.Event.Abstractions.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.Event.Abstractions.Extensions
{
    public static class EventServiceCollectionExtension
    {
        public static IServiceCollection AddMySqlDomainEventStorage(this IServiceCollection services)
        {
            services.AddSingleton<IEventSerializer, EventSerializer>();

            return services;
        }
    }
}
