using MDA.Domain.Commands;
using MDA.Domain.Events;
using MDA.Domain.Models;
using MDA.MessageBus.Disruptor;
using MDA.Shared.Serialization;
using MDA.Shared.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using MDA.StateBackend.MySql;

namespace EBank.BusinessServer.Bootstrap
{
    public class Bootstrapper
    {
        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            var assemblies = new[]
            {
                Assembly.GetExecutingAssembly()
            };

            // 1. 基础服务
            services.AddSerialization();
            services.AddTypes();

            services.AddMessageBusDisruptor(assemblies);

            services.AddDomainCommandServices();
            services.AddDomainModelServices();
            services.AddDomainEventServices();

            services.AddStateBackendMySql(context.Configuration);
        }
    }
}
