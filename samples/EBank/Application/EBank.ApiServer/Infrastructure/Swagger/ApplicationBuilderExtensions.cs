using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace EBank.ApiServer.Infrastructure.Swagger
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseApiDocuments(this IApplicationBuilder app)
        {
            var environment = app.ApplicationServices.GetService<IWebHostEnvironment>();

            if (environment.IsProduction()) return app;

            var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
            var configuration = app.ApplicationServices.GetService<IConfiguration>();

            app.UseSwagger(options =>
            {
                options.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    var applicationOrigin = configuration["ApplicationOrigin"];
                    var requestOrigin = $"{httpReq.Scheme}://{httpReq.Host}";

                    swagger.Servers = new List<OpenApiServer> { new OpenApiServer
                    {
                        Url = string.IsNullOrWhiteSpace(applicationOrigin) ? requestOrigin : applicationOrigin,
                        Description = environment.EnvironmentName }
                    };
                });
            });

            app.UseKnife4UI(options =>
            {
                options.DocumentTitle = "EBank API Documents";
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"EBank.API.{description.GroupName.ToUpperInvariant()}");
                }
            });

            return app;
        }
    }
}
