using EBank.Domain.Models.Primitives;
using EBank.Domain.Models.Transferring;
using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Notifications.Accounts
{
    /// <summary>
    /// 转账交易已验证的领域通知
    /// </summary>
    public class TransferTransactionValidatedDomainNotification : SubTransactionDomainNotification<TransferTransactionId>
    {
        public TransferTransactionValidatedDomainNotification(TransferTransactionId transactionId, Money money, TransferAccountType accountType)
        {
            TransactionId = transactionId;
            AccountType = accountType;
            Money = money;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public TransferTransactionId TransactionId { get; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public Money Money { get; }

        /// <summary>
        /// 账户类型
        /// </summary>
        public TransferAccountType AccountType { get; }
    }
}
