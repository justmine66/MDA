using Microsoft.Extensions.DependencyInjection;

namespace MDA.Domain.DependencyInjection
{
    public class DefaultDomainConfigureContext : IDomainConfigureContext
    {
        public DefaultDomainConfigureContext(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
