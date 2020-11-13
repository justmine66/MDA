//using MDA.Application.DependencyInjection;
//using MDA.Domain.DependencyInjection;
//using MDA.Infrastructure.DependencyInjection;
//using MDA.MessageBus;
//using MDA.MessageBus.DependencyInjection;
//using MDA.MessageBus.Disruptor;
//using MDA.StateBackend.MySql;
//using MDA.StateBackend.RDBMS.Shared.DependencyInjection;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Reflection;
//using Xunit.DependencyInjection;

//namespace MDA.XUnitTest
//{
//    public class Startup
//    {
//        public void ConfigureHost(IHostBuilder hostBuilder)
//            => hostBuilder.ConfigureAppConfiguration(builder => builder.AddJsonFile("appsettings.json"));

//        public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
//        {
//            var assemblies = new[]
//            {
//                Assembly.GetExecutingAssembly(),
//                Assembly.Load("MDA.Application")
//            };

//            services.AddLogging();

//            services.AddMdaServices(ctx =>
//            {
//                ctx.AddInfrastructure();
//                ctx.AddMessageBus(bus => bus.UseDisruptor(), assemblies);
//                ctx.AddStateBackend(state => state.UseMySql(context.Configuration));
//                ctx.AddApplication(app => app.UseMessageBus(MessageBusClientNames.Kafka));
//                ctx.AddDomain(domain => domain.UseMessageBus(MessageBusClientNames.Kafka), context.Configuration);
//            });
//        }

//        public void Configure(IServiceProvider provider, ILoggerFactory loggerFactory, ITestOutputHelperAccessor accessor)
//        {
//            ConfigureMessageBus(provider);
//        }

//        private void ConfigureMessageBus(IServiceProvider provider)
//        {
//            var queueService = provider.GetService<IMessageQueueService>();

//            queueService.Start();
//        }
//    }
//}
