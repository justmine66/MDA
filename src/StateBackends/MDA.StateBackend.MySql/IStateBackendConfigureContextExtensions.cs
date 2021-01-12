using MDA.Domain.Events;
using MDA.Domain.Models;
using MDA.StateBackend.RDBMS.Shared;
using MDA.StateBackend.RDBMS.Shared.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.StateBackend.MySql
{
    public static class IStateBackendConfigureContextExtensions
    {
        public static IStateBackendConfigureContext AddMySql(this IStateBackendConfigureContext context, IConfiguration configuration)
        {
            context.Services.AddSingleton<IRelationalDbStorageFactory, MySqlRelationalDbStorageFactory>();
            context.Services.AddSingleton<IDomainEventStateBackend, MySqlDomainEventStateBackend>();
            context.Services.AddSingleton(typeof(IAggregateRootCheckpointStateBackend<>), typeof(MySqlAggregateRootCheckpointStateBackend<>));

            context.Services.Configure<MySqlStateBackendOptions>(_ => { });

            if (configuration == null) return context;

            var stateBackendOptions = configuration.GetSection("StateBackends");

            context.Services.Configure<MySqlStateBackendOptions>(stateBackendOptions.GetSection("MySql"));

            return context;
        }
    }
}
