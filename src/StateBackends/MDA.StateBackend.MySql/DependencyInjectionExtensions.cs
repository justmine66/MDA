using MDA.Domain.Events;
using MDA.StateBackend.RDBMS.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.StateBackend.MySql
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddStateBackendMySql(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRelationalDbStorageFactory, MySqlRelationalDbStorageFactory>();
            services.AddSingleton<IDomainEventStateBackend, MySqlDomainEventStateBackend>();

            services.Configure<MySqlStateBackendOptions>(_ => { });
            if (configuration != null)
            {
                services.Configure<MySqlStateBackendOptions>(configuration.GetSection(nameof(MySqlStateBackendOptions)));
            }

            return services;
        }
    }
}
