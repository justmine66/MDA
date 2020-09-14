using MDA.MessageBus;
using MDA.XUnitTest.MessageBus;
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
        {

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddMessageBus(Assembly.GetExecutingAssembly());
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

            var queueService = provider.GetService<IMessageQueueService>();

            queueService.Start();
        }
    }
}
