using Microsoft.Extensions.DependencyInjection;

namespace EBank.Application
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddEBankAppServices(this IServiceCollection services)
        {
            services.AddSingleton<IEBankApplicationService, EBankApplicationService>();
            
            return services;
        }
    }
}
