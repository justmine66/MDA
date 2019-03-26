using MDA.Cluster;
using MDA.Concurrent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MDA
{
    public static class ServiceCollectionExtension
    {
        public static void AddMdaServices(this IServiceCollection container, IConfiguration configuration)
        {
            container.AddOptions();
            container.Configure<DisruptorOptions>(configuration.GetSection(nameof(DisruptorOptions)));
            container.Configure<ClusterSetting>(configuration.GetSection(nameof(ClusterSetting)));
            container.Configure<MdaOptions>(configuration.GetSection(nameof(MdaOptions)));
        }
    }
}
