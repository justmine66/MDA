using MDA.Application.Commands;
using MDA.Application.Notifications;
using MDA.Domain.Commands;
using MDA.Domain.Models;
using MDA.MessageBus;
using MDA.MessageBus.Disruptor;
using MDA.XUnitTest.ApplicationCommands;
using MDA.XUnitTest.ApplicationNotifications;
using MDA.XUnitTest.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using MDA.Domain.Events;
using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;

namespace MDA.XUnitTest
{
    public class Startup
    {
        public void ConfigureHost(IHostBuilder hostBuilder)
        {

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddMessageBusDisruptor();

            services.AddApplicationNotifications();
            services.AddTransient<IApplicationNotificationHandler<FakeApplicationNotification>, FakeApplicationNotificationHandler>();

            services.AddApplicationCommandCore();
            services.AddApplicationCommand<CreateApplicationCommand>();
            services.AddSingleton<IApplicationCommandHandler<CreateApplicationCommand>, CreateApplicationCommandHandler>();

            services.AddDomainCommands();
            services.AddDomainModels();
            services.AddDomainEvents();
           
            services.AddTransient<IMessageHandler<FakeMessage>, FakeMessageHandler>();
            services.AddTransient<IMessageHandler<FakeMessageWithPartitionKey>, FakeMessageWithPartitionKeyHandler>();
        }

        public void Configure(IServiceProvider provider, ILoggerFactory loggerFactory, ITestOutputHelperAccessor accessor)
        {
            loggerFactory.AddProvider(new XunitTestOutputLoggerProvider(accessor));

            ConfigureMessageBus(provider);
        }

        private void ConfigureMessageBus(IServiceProvider provider)
        {
            var subscriber = provider.GetService<IMessageSubscriber>();

            subscriber.Subscribe<FakeMessage, IMessageHandler<FakeMessage>>();
            subscriber.Subscribe<FakeMessageWithPartitionKey, IMessageHandler<FakeMessageWithPartitionKey>>();
            subscriber.Subscribe<FakeApplicationNotification, IApplicationNotificationHandler<FakeApplicationNotification>>();
            subscriber.Subscribe<CreateApplicationCommand, IMessageHandler<CreateApplicationCommand>>();

            subscriber.SubscribeDomainCommands();

            var queueService = provider.GetService<IMessageQueueService>();

            queueService.Start();
        }
    }
}
