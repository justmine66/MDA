using MDA.Cluster;
using MDA.Concurrent;
using MDA.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace MDA
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMdaServices(this IServiceCollection container)
        {
            return AddMdaServices(container, builder => { });
        }

        public static IServiceCollection AddMdaServices(this IServiceCollection container, Action<IMdaBuilder> configure)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.AddOptions();

            container.TryAdd(ServiceDescriptor.Singleton(typeof(IInboundDisruptor<>), typeof(InboundDisruptorImpl<>)));
            container.AddSingleton<IAppStateProvider, AppStateProvider>();

            container.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<MdaOptions>>(
                new MdaOptionsConfigure(ClusterSettingFactory.Create(),DisruptorOptionsFactory.Create())));
            container.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<DisruptorOptions>>(
                new DisruptorOptionsConfigure()));
            container.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<ClusterSettings>>(
                new ClusterSettingsConfigure(ClusterSettingFactory.Create().AppMode)));

            configure(new MdaBuilder(container));

            return container;
        }
    }
}
