using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace MDA.Shared.Configurations
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMdaServices(this IServiceCollection container)
        {
            return AddMdaServices(container, builder => { });
        }

        public static IServiceCollection AddMdaServices(this IServiceCollection container, Action<IMdaApplicationBuilder> configure)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.AddOptions();

            //container.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<MdaOptions>>(new MdaOptionsConfigure(ClusterSettingFactory.Create(), DisruptorOptionsFactory.Create())));
            //container.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<DisruptorOptions>>(
            //    new DisruptorOptionsConfigure()));
            //container.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<ClusterSettings>>(
            //    new ClusterSettingsConfigure(ClusterSettingFactory.Create().AppMode)));

            configure(new MdaApplicationBuilder(container));

            return container;
        }
    }
}
