using MDA.Cluster;
using MDA.Concurrent;
using Microsoft.Extensions.Options;

namespace MDA
{
    public class DefaultMdaConfigureOption : ConfigureOptions<MdaOptions>
    {
        public DefaultMdaConfigureOption(ClusterSettings clusterSetting, DisruptorOptions disruptorOptions)
            : base(options =>
            {
                options.ClusterSetting = clusterSetting;
                options.DisruptorOptions = disruptorOptions;
            })
        {
        }
    }
}
