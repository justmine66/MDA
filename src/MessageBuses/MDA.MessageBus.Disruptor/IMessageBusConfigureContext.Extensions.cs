using MDA.MessageBus.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.MessageBus.Disruptor
{
    public static class MessageBusConfigureContextExtensions
    {
        public static IMessageBusConfigureContext UseDisruptor(this IMessageBusConfigureContext context)
        {
            context.Services.AddSingleton<IMessageQueueService, DisruptorMessageQueueService>();
            context.Services.AddSingleton<IAsyncMessageQueueService, DisruptorAsyncMessageQueueService>();

            return context;
        }
    }
}
