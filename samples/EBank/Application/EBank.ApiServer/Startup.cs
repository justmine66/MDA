using EBank.ApiServer.Application;
using EBank.ApiServer.Infrastructure;
using EBank.ApiServer.Infrastructure.Swagger;
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
using System.Reflection;

namespace EBank.ApiServer
{
    public class Startup
    {
        private static readonly Assembly[] Assemblies = { Assembly.GetExecutingAssembly() };

        public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            HostEnvironment = hostEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiDocuments(HostEnvironment);
            services.AddApiVersionServices();
            services.AddControllerServices();

            services.AddMda(ctx =>
            {
                ctx.AddInfrastructure();
                ctx.AddMessageBus(bus => bus.AddKafka(Configuration), Assemblies);

                ctx.AddApplication(app => app.UseMessageBus(MessageBusClientNames.Kafka, Configuration));
                ctx.AddDomain(domain => domain.UseMessageBus(MessageBusClientNames.Kafka, Configuration), Configuration, Assemblies);

                ctx.AddStateBackend(state => state.AddMySql(Configuration));
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

            app.UseApiDocuments();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapSwagger("{documentName}/api-docs");
            });
        }
    }
}
