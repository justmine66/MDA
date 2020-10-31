using Microsoft.Extensions.DependencyInjection;

namespace MDA.StateBackend.RDBMS.Shared.DependencyInjection
{
    public class DefaultStateBackendConfigureContext : IStateBackendConfigureContext
    {
        public DefaultStateBackendConfigureContext(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
