using Microsoft.Extensions.DependencyInjection;

namespace MDA.Infrastructure.DependencyInjection
{
    public interface IMdaConfigureContext
    {
        IServiceCollection Services { get; }
    }
}
