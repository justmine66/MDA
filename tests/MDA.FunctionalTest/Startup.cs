using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

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
                    });
                    builder.Configure(Configure);
                });
        }

        private void Configure(IApplicationBuilder app)
        {
            // 不能忽略，否则测试服务无法启动。
        }
    }
}
