namespace MDA.Cluster
{
    public class ClusterSettingFactory
    {
        public static ClusterSetting Create()
        {
            return new ClusterSetting() { AppMode = new AppMode() { Environment = new Environment() } };
        }
    }
}
