using System;

namespace MDA.Saga
{
    /// <summary>
    /// 恢复策略
    /// </summary>
    [Flags]
    public enum RecoveryStrategy
    {
        /// <summary>
        /// 向后恢复。
        /// 如果任一子事务失败，补偿所有已完成的事务。
        /// </summary>
        Backward = 1 << 0,

        /// <summary>
        /// 向前恢复，适用于必须要成功的场景。
        /// 重试失败的事务，假设每个子事务最终都会成功。
        /// </summary>
        Forward = 1 << 1
    }
}
