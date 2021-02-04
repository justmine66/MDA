using EBank.Domain.Models.Accounts.Primitives;

namespace EBank.Domain.Models.Transferring
{
    /// <summary>
    /// 转账账户
    /// </summary>
    public class TransferAccount
    {
        public TransferAccount(BankAccountId id, BankAccountName name, BankName bank, TransferAccountType accountType)
        {
            Id = id;
            Name = name;
            Bank = bank;
            AccountType = accountType;
        }

        /// <summary>
        /// 账户类型
        /// </summary>
        public TransferAccountType AccountType { get; }

        /// <summary>
        /// 账户号
        /// </summary>
        public BankAccountId Id { get; }

        /// <summary>
        /// 账户名
        /// </summary>
        public BankAccountName Name { get; }

        /// <summary>
        /// 开户行
        /// </summary>
        public BankName Bank { get; }

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
