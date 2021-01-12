﻿using EBank.Domain.Repositories.MySql;
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

namespace EBank.BusinessServer.Bootstrap
{
    public class Bootstrapper
    {
        private static readonly Assembly CurrentAssembly = Assembly.GetExecutingAssembly();

        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddMda(ctx =>
            {
                ctx.AddInfrastructure();
                ctx.AddMessageBus(bus => bus.AddDisruptor().AddKafka(context.Configuration), CurrentAssembly);

                ctx.AddApplication(app => app.UseMessageBus(MessageBusClientNames.Kafka), CurrentAssembly);
                ctx.AddDomain(domain => domain.UseMessageBus(MessageBusClientNames.Kafka, context.Configuration), context.Configuration);

                ctx.AddStateBackend(state => state.AddMySql(context.Configuration));
            });

            services.AddSingleton<IBankAccountRepository, MySqlBankAccountRepository>();
        }
    }
}
