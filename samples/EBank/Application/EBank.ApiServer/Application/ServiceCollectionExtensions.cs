using EBank.ApiServer.Application.Business;
using EBank.ApiServer.Application.Querying;
using Microsoft.Extensions.DependencyInjection;

namespace EBank.ApiServer.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEBankAppServices(this IServiceCollection services)
        {
            services.AddSingleton<IEBankApplicationService, DefaultEBankApplicationService>();
            services.AddSingleton<IBankAccountQueryService, MySqlBankAccountQueryService>();

            return services;
        }
    }
}
