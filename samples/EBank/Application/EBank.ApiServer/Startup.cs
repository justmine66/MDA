using EBank.Application;
using EBank.Application.Querying;
using EBank.Domain.Models.Accounts;
using EBank.Domain.MySql;
using MDA.Application.Commands;
using MDA.Application.Notifications;
using MDA.Domain.Commands;
using MDA.Domain.Events;
using MDA.Domain.Models;
using MDA.MessageBus.Disruptor;
using MDA.Shared.Serialization;
using MDA.Shared.Types;
using MDA.StateBackend.MySql;
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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var assemblies = new[]
            {
                Assembly.GetExecutingAssembly(),
                Assembly.Load("EBank.Application.BusinessServer"),
                Assembly.Load("EBank.Application.Querying")
            };

            // 1. 基础服务
            services.AddSerialization();
            services.AddTypes();

            services.AddMessageBusDisruptor(assemblies);

            // 2. 应用层服务
            services.AddApplicationNotificationServices();
            services.AddApplicationCommandServices(assemblies);

            // 3. 领域层服务
            services.AddDomainCommandServices();
            services.AddDomainModelServices();
            services.AddDomainEventServices();

            services.AddStateBackendMySql(Configuration);

            // 4. 电子银行应用服务
            services.AddEBankAppServices();
            services.AddSingleton<IBankAccountRepository, MySqlBankAccountRepository>();
            //services.AddSingleton<IBankAccountQueryService, MySqlBankAccountQueryService>();

            // 5. 本地服务
            services.AddHostedService<StartupHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
