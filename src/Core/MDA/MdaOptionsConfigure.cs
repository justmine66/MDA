using MDA.Cluster;
using MDA.Concurrent;
using Microsoft.Extensions.Options;

namespace MDA
{
    public class MdaOptionsConfigure : ConfigureOptions<MdaOptions>
    {
        public MdaOptionsConfigure(ClusterSettings clusterSetting, DisruptorOptions disruptorOptions)
            : base(options =>
            {
                options.ClusterSetting = clusterSetting;
                options.DisruptorOptions = disruptorOptions;
            })
        {
        }
    }
}
