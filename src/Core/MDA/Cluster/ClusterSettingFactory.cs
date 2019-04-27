namespace MDA.Cluster
{
    public class ClusterSettingFactory
    {
        public static ClusterSettings Create()
        {
            return new ClusterSettings() { AppMode = new AppMode() { Environment = new Environment() } };
        }
    }
}
