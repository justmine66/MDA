using Microsoft.Extensions.DependencyInjection;

namespace MDA.Shared.Configurations
{
    /// <summary>
    /// An interface for configuring mda providers.
    /// </summary>
    public interface IMdaApplicationBuilder
    {
        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> where mda services are configured.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
