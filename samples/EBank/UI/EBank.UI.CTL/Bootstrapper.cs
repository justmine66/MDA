using Autofac;
using EBank.Application;
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
            services.AddMessageBusDisruptor();

            services.AddSerialization();
            services.AddTypes();

            services.AddApplicationNotifications();

            services.AddApplicationCommandCore();

            services.AddDomainCommands();
            services.AddDomainModels();
            services.AddDomainEvents();

            services.AddMySql(context.Configuration);

            services.AddEBankAppServices();

            services.AddHostedService<ConfigureHostedService>();
            services.AddHostedService<StartupHostedService>();
        }

        public static void ConfigureContainer(HostBuilderContext context, ContainerBuilder container)
        {

        }
    }
}
