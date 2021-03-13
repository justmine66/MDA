using EBank.Domain.Models.Accounts;
using MDA.Application.DependencyInjection;
using MDA.Domain.DependencyInjection;
using MDA.Infrastructure.DependencyInjection;
using MDA.MessageBus;
using MDA.MessageBus.DependencyInjection;
using MDA.MessageBus.Disruptor;
using MDA.MessageBus.Kafka;
using MDA.StateBackend.MySql;
using MDA.StateBackend.RDBMS.Shared.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using EBank.Domain.MySql;

namespace EBank.DomainServer.Bootstrap
{
    public class Bootstrapper
    {
        private static readonly Assembly[] Assemblies = { Assembly.GetExecutingAssembly(), Assembly.Load("EBank.Domain") };

        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            var configuration = context.Configuration;

            services.AddMda(ctx =>
            {
                ctx.AddInfrastructure();
                ctx.AddMessageBus(configure => configure.AddDisruptor().AddKafka(configuration), Assemblies);

                ctx.AddApplication(app => app.UseMessageBus(MessageBusClientNames.Kafka), Assemblies);
                ctx.AddDomain(domain => domain.UseMessageBus(MessageBusClientNames.Kafka, configuration), configuration, Assemblies);

                ctx.AddStateBackend(state => state.AddMySql(configuration));
            });

            services.AddSingleton<IBankAccountRepository, MySqlBankAccountRepository>();
        }
    }
}
