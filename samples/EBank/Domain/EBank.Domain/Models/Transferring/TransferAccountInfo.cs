namespace EBank.Domain.Models.Transferring
{
    /// <summary>
    /// 转账账户信息，值对象
    /// </summary>
    public class TransferAccountInfo
    {
        public TransferAccountInfo(long id, string name, string bank)
        {
            Id = id;
            Name = name;
            Bank = bank;
        }

        /// <summary>
        /// 账户号
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// 账户名
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 开户行
        /// </summary>
        public string Bank { get; }

        /// <summary>
        /// 已验证
        /// </summary>
        public bool Validated { get; private set; }

        /// <summary>
        /// 有效
        /// </summary>
        public void SetValidated() => Validated = true;
    }
}
