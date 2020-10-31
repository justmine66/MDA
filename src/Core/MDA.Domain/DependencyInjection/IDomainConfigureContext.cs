using Microsoft.Extensions.DependencyInjection;

namespace MDA.Domain.DependencyInjection
{
    public interface IDomainConfigureContext
    {
        IServiceCollection Services { get; }
    }
}
