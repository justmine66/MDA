using Microsoft.Extensions.DependencyInjection;

namespace MDA.StateBackend.RDBMS.Shared.DependencyInjection
{
    public interface IStateBackendConfigureContext
    {
        IServiceCollection Services { get; }
    }
}
