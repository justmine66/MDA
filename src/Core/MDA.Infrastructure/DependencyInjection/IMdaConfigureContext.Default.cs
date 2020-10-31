using Microsoft.Extensions.DependencyInjection;

namespace MDA.Infrastructure.DependencyInjection
{
    class DefaultMdaConfigureContext : IMdaConfigureContext
    {
        public DefaultMdaConfigureContext(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
