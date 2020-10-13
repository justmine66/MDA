using MDA.Application.Commands;
using MDA.Application.Notifications;
using MDA.Domain.Commands;
using MDA.Domain.Events;
using MDA.Domain.Models;
using MDA.MessageBus;
using MDA.MessageBus.Disruptor;
using MDA.Shared.Serialization;
using MDA.Shared.Types;
using MDA.StateBackend.MySql;
using MDA.XUnitTest.ApplicationCommands;
using MDA.XUnitTest.ApplicationNotifications;
using MDA.XUnitTest.MessageBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;

namespace MDA.XUnitTest
{
    public class Startup
    {
        public void ConfigureHost(IHostBuilder hostBuilder)
            => hostBuilder.ConfigureAppConfiguration(builder => builder.AddJsonFile("appsettings.json"));

        public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
        {
            var assemblies = new[] { Assembly.GetExecutingAssembly() };

            services.AddLogging();
            services.AddMessageBusDisruptor(assemblies);
            services.AddSerialization();
            services.AddTypes();

            services.AddApplicationNotifications();
            services.AddScoped<IApplicationNotificationHandler<FakeApplicationNotification>, FakeApplicationNotificationHandler>();

            services.AddApplicationCommandCore();
            services.AddApplicationCommand<CreateApplicationCommand>();
            services.AddScoped<IApplicationCommandHandler<CreateApplicationCommand>, CreateApplicationCommandHandler>();
            services.AddScoped<IAsyncApplicationCommandHandler<CreateApplicationCommand>, CreateApplicationCommandHandler>();

            services.AddDomainCommands();
            services.AddDomainModels();
            services.AddDomainEvents();

            services.AddStateBackendMySql(context.Configuration);

            //services.AddScoped<IMessageHandler<FakeMessage>, FakeMessageHandler>();
            //services.AddScoped<IAsyncMessageHandler<FakeMessage>, AsyncFakeMessageHandler>();
            //services.AddScoped<IMessageHandler<FakeMessageWithPartitionKey>, FakeMessageWithPartitionKeyHandler>();
            //services.AddScoped<IMessageHandler<FakeMessage>, MultiMessageHandler>();
            //services.AddScoped<IAsyncMessageHandler<FakeMessage>, MultiMessageHandler>();
        }

        public void Configure(IServiceProvider provider, ILoggerFactory loggerFactory, ITestOutputHelperAccessor accessor)
        {
            loggerFactory.AddProvider(new XunitTestOutputLoggerProvider(accessor));

            ConfigureMessageBus(provider);
        }

        private void ConfigureMessageBus(IServiceProvider provider)
        {
            //var subscriber = provider.GetService<IMessageSubscriberManager>();

            //subscriber.Subscribe<FakeMessage, IMessageHandler<FakeMessage>>();
            //subscriber.Subscribe<FakeMessage, IMessageHandler<FakeMessage>>();
            //subscriber.Subscribe<FakeMessage, IMessageHandler<FakeMessage>>();
            //subscriber.SubscribeAsync<FakeMessage, IAsyncMessageHandler<FakeMessage>>();
            //subscriber.Subscribe<FakeMessageWithPartitionKey, IMessageHandler<FakeMessageWithPartitionKey>>();
            //subscriber.Subscribe<FakeApplicationNotification, IApplicationNotificationHandler<FakeApplicationNotification>>();
            //subscriber.Subscribe<CreateApplicationCommand, IMessageHandler<CreateApplicationCommand>>();

            //subscriber.SubscribeDomainCommands();

            var queueService = provider.GetService<IMessageQueueService>();

            queueService.Start();
        }
    }
}
