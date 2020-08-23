using MDA.Domain.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDomainCommandPublisher, DisruptorDomainCommandPublisher>();
        }
    }
}
