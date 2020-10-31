using MDA.Application.Commands;
using MDA.Application.Notifications;
using MDA.Infrastructure.DependencyInjection;
using System;
using System.Reflection;

namespace MDA.Application.DependencyInjection
{
    public static class MdaConfigureContextExtensions
    {
        public static IMdaConfigureContext AddApplication(
            this IMdaConfigureContext context,
            Action<IApplicationConfigureContext> configure, 
            params Assembly[] assemblies)
        {
            context.Services.AddApplicationCommandCore(assemblies);
            context.Services.AddApplicationNotificationCore();

            configure(new DefaultApplicationConfigureContext(context.Services));

            return context;
        }
    }
}
