using MDA.Event;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.EventStore.MySql.Extensions
{
    public static class DomainEventStoreServiceCollectionExtension
    {
        public static IServiceCollection AddMySqlDomainEventStorage(this IServiceCollection services, IConfiguration config, string eventStoreOptionKey = "MySqlDomainEventStoreOptions")
        {
            services.Configure<MySqlDomainEventStoreOptions>(config.GetSection(eventStoreOptionKey));
            services.AddScoped<IDomainEventStore, MySqlDomainEventStore>();

            return services;
        }
    }
}
