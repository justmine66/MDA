using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;

namespace EBank.ApiServer.Infrastructure.Swagger
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiDocuments(this IServiceCollection services, IWebHostEnvironment environment)
        {
            if (environment.IsProduction()) return services;

            var xmlFile = $"{typeof(ServiceCollectionExtensions).Assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            services.AddSingleton<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath, true);
                }
                options.SchemaFilter<EnumSchemaFilter>();
                options.OperationFilter<SwaggerDefaultValues>();
            });

            return services;
        }
    }
}
