﻿using MDA.Domain.Commands;
using MDA.Domain.Events;
using MDA.Domain.Models;
using MDA.Domain.Notifications;
using MDA.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;

namespace MDA.Domain.DependencyInjection
{
    public static class MdaConfigureContextExtensions
    {
        public static IMdaConfigureContext AddDomain(
            this IMdaConfigureContext context,
            Action<IDomainConfigureContext> configure,
            IConfiguration configuration)
        {
            context.Services.AddDomainCommandCore();
            context.Services.AddDomainModelCore(configuration); 
            context.Services.AddDomainEventCore();
            context.Services.AddDomainNotificationCore();

            configure(new DefaultDomainConfigureContext(context.Services));

            return context;
        }
    }
}
