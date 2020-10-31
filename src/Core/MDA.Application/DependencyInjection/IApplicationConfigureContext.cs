using Microsoft.Extensions.DependencyInjection;

namespace MDA.Application.DependencyInjection
{
    public interface IApplicationConfigureContext
    {
        IServiceCollection Services { get; }
    }
}
