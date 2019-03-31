using Microsoft.Extensions.Options;

namespace MDA.Cluster
{
    public class DefaultClusterSettingsConfigureOption : ConfigureOptions<ClusterSettings>
    {
        public DefaultClusterSettingsConfigureOption(AppMode mode)
            : base(options => options.AppMode = mode)
        {
        }
    }
}
