using Microsoft.Extensions.DependencyInjection;

namespace MDA
{
    internal class MdaBuilder : IMdaBuilder
    {
        public MdaBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
