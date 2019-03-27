using MDA.Cluster;
using MDA.Concurrent;

namespace MDA
{
    public class MdaOptionsFactory
    {
        public static MdaOptions Create()
        {
            return new MdaOptions()
            {
                ClusterSetting = new ClusterSetting() { AppMode = new AppMode() },
                DisruptorOptions = new DisruptorOptions()
            };
        }
    }
}
