using MDA.Domain.Events;
using MDA.StateBackend.RDBMS.Shared;
using MDA.StateBackend.RDBMS.Shared.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.StateBackend.MySql
{
    public static class IStateBackendConfigureContextExtensions
    {
        public static IStateBackendConfigureContext UseMySql(this IStateBackendConfigureContext context, IConfiguration configuration)
        {
            context.Services.AddSingleton<IRelationalDbStorageFactory, MySqlRelationalDbStorageFactory>();
            context.Services.AddSingleton<IDomainEventStateBackend, MySqlDomainEventStateBackend>();

            context.Services.Configure<MySqlStateBackendOptions>(_ => { });
            if (configuration != null)
            {
                context.Services.Configure<MySqlStateBackendOptions>(configuration.GetSection(nameof(MySqlStateBackendOptions)));
            }

            return context;
        }
    }
}
