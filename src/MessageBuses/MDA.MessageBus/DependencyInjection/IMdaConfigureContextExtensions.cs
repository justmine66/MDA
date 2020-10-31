using MDA.Infrastructure.DependencyInjection;
using System;
using System.Reflection;

namespace MDA.MessageBus.DependencyInjection
{
    public static class MdaConfigureContextExtensions
    {
        public static IMdaConfigureContext AddMessageBus(
            this IMdaConfigureContext context,
            Action<IMessageBusConfigureContext> configure, 
            params Assembly[] assemblies)
        {
            context.Services.AddMessageBusCore(assemblies);

            configure(new DefaultMessageBusConfigureContext(context.Services));

            return context;
        }
    }
}
