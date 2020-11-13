using System.IO;
using EBank.ApiServer.Application;
using MDA.Application.DependencyInjection;
using MDA.Domain.DependencyInjection;
using MDA.Infrastructure.DependencyInjection;
using MDA.MessageBus;
using MDA.MessageBus.DependencyInjection;
using MDA.MessageBus.Kafka;
using MDA.StateBackend.MySql;
using MDA.StateBackend.RDBMS.Shared.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace EBank.ApiServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "EBank HTTP API",
                    Version = "v1",
                    Description = "The EBank Service HTTP API"
                });

                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var xmlPath = Path.Combine(basePath, "EBank.ApiServer.xml");
                options.IncludeXmlComments(xmlPath);
            });

            services.AddMdaServices(ctx =>
            {
                ctx.AddInfrastructure();
                ctx.AddMessageBus(bus => bus.UseKafka(Configuration), Assembly.GetExecutingAssembly());
                ctx.AddApplication(app => app.UseMessageBus(MessageBusClientNames.Kafka, Configuration));
                ctx.AddDomain(domain => domain.UseMessageBus(MessageBusClientNames.Kafka), Configuration);
                ctx.AddStateBackend(state => state.UseMySql(Configuration));
            });

            // 4. 电子银行应用服务
            services.AddEBankAppServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger()
                .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EBank API V1");
                c.OAuthClientId("ebank");
                c.OAuthAppName("EBank Swagger UI");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
