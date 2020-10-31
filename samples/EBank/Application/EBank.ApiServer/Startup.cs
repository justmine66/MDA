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

            services.AddMdaServices(ctx =>
            {
                ctx.AddInfrastructure();
                ctx.AddMessageBus(bus => bus.UseKafka(Configuration), Assembly.GetExecutingAssembly());
                ctx.AddStateBackend(state => state.UseMySql(Configuration));
                ctx.AddApplication(app => app.UseMessageBus(MessageBusClientNames.Kafka));
                ctx.AddDomain(domain => domain.UseMessageBus(MessageBusClientNames.Kafka), Configuration);
            });

            // 4. 电子银行应用服务
            services.AddEBankAppServices();
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
