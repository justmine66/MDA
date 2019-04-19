using Microsoft.Extensions.Options;

namespace MDA.Cluster
{
    public class ClusterSettingsConfigure : ConfigureOptions<ClusterSettings>
    {
        public ClusterSettingsConfigure(AppMode mode)
            : base(options => options.AppMode = mode)
        {
        }
    }
}
