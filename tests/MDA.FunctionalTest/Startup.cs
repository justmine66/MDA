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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace MDA.FunctionalTest
{
    public class Startup
    {
        public void ConfigureHost(IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureWebHostDefaults(builder =>
                {
                    builder.UseTestServer();
                    builder.ConfigureServices((context, services) =>
                    {
                        services.TryAddSingleton(sp => sp.GetRequiredService<IHost>().GetTestClient());
                        services.TryAddSingleton<EBandApiTestFixture>();
                        ConfigureServices(services, context.Configuration);
                    });
                    builder.Configure(Configure);
                });
        }

        private void Configure(IApplicationBuilder app)
        {
            app.UseResponseCaching();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddResponseCaching();

            services.AddControllers(options =>
            {
                options.CacheProfiles.Add("default", new CacheProfile()
                {
                    Duration = 300,
                    VaryByQueryKeys = new[] { "*" }
                });
                options.CacheProfiles.Add("private", new CacheProfile()
                {
                    Duration = 300,
                    Location = ResponseCacheLocation.Client,
                    VaryByQueryKeys = new[] { "*" }
                });
                options.CacheProfiles.Add("noCache", new CacheProfile()
                {
                    NoStore = true
                });
            })
                .AddApplicationPart(typeof(EBank.ApiServer.Program).Assembly)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc; // 设置时区为 UTC
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddMda(ctx =>
            {
                ctx.AddInfrastructure();
                ctx.AddMessageBus(bus => bus.AddKafka(configuration), Assembly.GetExecutingAssembly());
                ctx.AddApplication(app => app.UseMessageBus(MessageBusClientNames.Kafka, configuration));
                ctx.AddDomain(domain => domain.UseMessageBus(MessageBusClientNames.Kafka), configuration);
                ctx.AddStateBackend(state => state.AddMySql(configuration));
            });

            // 4. 电子银行应用服务
            services.AddEBankAppServices();
        }
    }
}
