using MDA.Domain.Commands;
using MDA.Domain.Events;
using MDA.Domain.Exceptions;
using MDA.Domain.Models;
using MDA.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace MDA.Domain.DependencyInjection
{
    public static class MdaConfigureContextExtensions
    {
        public static IMdaConfigureContext AddDomain(
            this IMdaConfigureContext context,
            Action<IDomainConfigureContext> configure,
            IConfiguration configuration, 
            params Assembly[] assemblies)
        {
            context.Services.AddDomainCommandCore();
            context.Services.AddDomainModelCore(configuration); 
            context.Services.AddDomainEventCore();

            configure(new DefaultDomainConfigureContext(context.Services));

            return context;
        }
    }
}
