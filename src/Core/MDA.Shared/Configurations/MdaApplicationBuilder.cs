using Microsoft.Extensions.DependencyInjection;

namespace MDA.Shared.Configurations
{
    internal class MdaApplicationBuilder : IMdaApplicationBuilder
    {
        public MdaApplicationBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
