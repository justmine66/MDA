using EBank.ApiServer.Infrastructure.Filters;
using EBank.ApiServer.Models.Output;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace EBank.ApiServer.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddControllerServices(this IServiceCollection services)
        {
            services.AddControllers(options =>
                {
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                }).AddControllersAsServices()
                .ConfigureApiBehaviorOptions(e =>
                {
                    e.InvalidModelStateResponseFactory = actionContext =>
                    {
                        var errors = actionContext.ModelState
                            .Where(e1 => e1.Value.Errors.Count > 0)
                            .Select(e1 => e1.Value.Errors.First().ErrorMessage);

                        return new JsonResult(ApiErrorResult.BadRequest(errors));
                    };
                }); 

            return services;
        }

        public static IServiceCollection AddApiVersionServices(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true; // 当未指定版本时，默认访问v1.0的接口。
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}
