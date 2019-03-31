using Microsoft.Extensions.DependencyInjection;

namespace MDA
{
    /// <summary>
    /// An interface for configuring mda providers.
    /// </summary>
    public interface IMdaBuilder
    {
        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> where mda services are configured.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
