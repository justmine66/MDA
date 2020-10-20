namespace MDA.StateBackend.RDBMS.Shared
{
    public interface IRelationalDbStorageFactory
    {
        IRelationalDbStorage CreateRelationalDbStorage(DatabaseScheme scheme);
    }

    /// <summary>
    /// 数据库方案
    /// </summary>
    public enum DatabaseScheme
    {
        /// <summary>
        /// 视图数据库，即读库。
        /// </summary>
        ReadDb = 1 << 0,

        /// <summary>
        /// 状态数据库，即写库。
        /// </summary>
        StateDb = 1 << 1
    }
}
