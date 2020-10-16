using Autofac;
using EBank.Application;
using EBank.Application.Querying;
using EBank.Domain.Models.Accounts;
using EBank.Domain.MySql;
using MDA.Application.Commands;
using MDA.Application.Notifications;
using MDA.Domain.Commands;
using MDA.Domain.Events;
using MDA.Domain.Models;
using MDA.MessageBus.Disruptor;
using MDA.Shared.Serialization;
using MDA.Shared.Types;
using MDA.StateBackend.MySql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace EBank.UI.CTL
{
    public class Bootstrapper
    {
        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            var assemblies = new[]
            {
                Assembly.GetExecutingAssembly(),
                Assembly.Load("EBank.Application.BusinessServer")
            };

            // 1. 基础服务
            services.AddSerialization();
            services.AddTypes();

            services.AddMessageBusDisruptor(assemblies);

            // 2. 应用层服务
            services.AddApplicationNotificationServices();

            services.AddApplicationCommandServices(assemblies);

            // 3. 领域层服务
            services.AddDomainCommandServices();
            services.AddDomainModelServices();
            services.AddDomainEventServices();

            services.AddStateBackendMySql(context.Configuration);

            // 4. 电子银行应用服务
            services.AddEBankAppServices();
            services.AddSingleton<IBankAccountRepository, MySqlBankAccountRepository>();
            services.AddSingleton<IBankAccountQueryService, MySqlBankAccountQueryService>();

            // 5. 本地服务
            services.AddHostedService<StartupHostedService>();
            services.AddHostedService<MainHostedService>();
        }

        public static void ConfigureContainer(HostBuilderContext context, ContainerBuilder container)
        {

        }
    }
}
