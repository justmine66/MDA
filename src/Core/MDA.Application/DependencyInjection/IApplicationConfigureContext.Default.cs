using Microsoft.Extensions.DependencyInjection;

namespace MDA.Application.DependencyInjection
{
    public class DefaultApplicationConfigureContext : IApplicationConfigureContext
    {
        public DefaultApplicationConfigureContext(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
