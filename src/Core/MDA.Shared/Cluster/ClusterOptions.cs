namespace MDA.Shared.Cluster
{
    /// <summary>
    /// 集群配置项
    /// </summary>
    public class ClusterOptions
    {
        public Environment Environment { get; set; }
    }

    /// <summary>
    /// 环境信息
    /// </summary>
    public class Environment
    {
        public const string Leader = "Leader";
        public const string Follower = "Follower";

        /// <summary>
        /// 名称，默认为：Master。
        /// </summary>
        public string Name { get; set; } = Leader;

        public bool IsLeader => Name == Leader;

        public bool IsFollower => Name == Follower;
    }
}
