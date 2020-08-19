namespace EBank.Domain.Models.Transferring
{
    /// <summary>
    /// 转账交易账户信息，值对象
    /// </summary>
    public class TransferTransactionAccount
    {
        public TransferTransactionAccount(long id, string name, string bank)
        {
            Id = id;
            Name = name;
            Bank = bank;
            IsValidationPassed = false;
        }

        /// <summary>
        /// 账户号
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// 账户名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public string Bank { get; private set; }

        /// <summary>
        /// 是否已验证通过
        /// </summary>
        public bool IsValidationPassed { get; private set; }

        /// <summary>
        /// 通过验证
        /// </summary>
        public void PassValidation() => IsValidationPassed = true;
    }
}
