using Autofac;
using EBank.Application;
using EBank.Application.Commands.Accounts;
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

namespace EBank.UI.CTL
{
    public class Bootstrapper
    {
        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            // 1. 基础服务
            services.AddMessageBusDisruptor();

            services.AddSerialization();
            services.AddTypes();

            services.AddStateBackendMySql(context.Configuration);

            // 2. 应用层服务
            services.AddApplicationNotifications();

            services.AddApplicationCommandCore();
            services.AddApplicationCommand<OpenBankAccountApplicationCommand>();

            // 3. 领域层服务
            services.AddDomainCommands();
            services.AddDomainModels();
            services.AddDomainEvents();

            // 4. 电子银行应用服务
            services.AddEBankAppServices();
            services.AddSingleton<IBankAccountRepository, MySqlBankAccountRepository>();
            services.AddSingleton<IBankAccountQueryService, BankAccountQueryService>();

            // 5. 本地服务
            services.AddHostedService<StartupHostedService>();
            services.AddHostedService<MainHostedService>();
        }

        public static void ConfigureContainer(HostBuilderContext context, ContainerBuilder container)
        {

        }
    }
}
