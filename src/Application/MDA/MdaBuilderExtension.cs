using MDA.Cluster;
using MDA.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MDA
{
    /// <summary>
    /// Extension methods for setting up mda services in an <see cref="IMdaBuilder" />.
    /// </summary>
    public static class MdaBuilderExtension
    {
        public static IMdaBuilder AddMdaOptions(this IMdaBuilder builder, ClusterSettings clusterSettings, DisruptorOptions disruptorOptions)
        {
            builder.Services.Add(ServiceDescriptor.Singleton<IConfigureOptions<MdaOptions>>(
                new DefaultMdaConfigureOption(clusterSettings, disruptorOptions)));
            return builder;
        }

        public static IMdaBuilder AddDisruptorOptions(this IMdaBuilder builder, int inboundRingBufferSize, int outboundRingBufferSize)
        {
            builder.Services.Add(ServiceDescriptor.Singleton<IConfigureOptions<DisruptorOptions>>(
                new DefaultDisruptorConfigureOption(inboundRingBufferSize, outboundRingBufferSize)));
            return builder;
        }

        public static IMdaBuilder AddClusterSettings(this IMdaBuilder builder, AppMode mode)
        {
            builder.Services.Add(ServiceDescriptor.Singleton<IConfigureOptions<ClusterSettings>>(
                new DefaultClusterSettingsConfigureOption(mode)));
            return builder;
        }
    }
}
