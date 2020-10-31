using MDA.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;

namespace MDA.StateBackend.RDBMS.Shared.DependencyInjection
{
    public static class MdaConfigureContextExtensions
    {
        public static IMdaConfigureContext AddStateBackend(
            this IMdaConfigureContext context,
            Action<IStateBackendConfigureContext> configure)
        {
            configure(new DefaultStateBackendConfigureContext(context.Services));

            return context;
        }
    }
}
