namespace MDA.Cluster
{
    public class ClusterSettings
    {
        public AppMode AppMode { get; set; }
    }

    /// <summary>
    /// 应用程序运行模式
    /// </summary>
    public class AppMode
    {
        public Environment Environment { get; set; }
    }

    /// <summary>
    /// 环境信息
    /// </summary>
    public class Environment
    {
        /// <summary>
        /// 名称，默认为：Master。
        /// </summary>
        public string Name { get; set; } = "Master";

        public bool IsMaster => Name == "Master";
    }
}
