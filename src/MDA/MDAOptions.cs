using MDA.Cluster;
using MDA.Concurrent;

namespace MDA
{
    public class MdaOptions
    {
        public ClusterSetting ClusterSetting { get; set; }
        public DisruptorOptions DisruptorOptions { get; set; }
    }
}
