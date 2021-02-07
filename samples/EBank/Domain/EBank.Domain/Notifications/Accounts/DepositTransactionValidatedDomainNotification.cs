using EBank.Domain.Models.Depositing.Primitives;
using EBank.Domain.Models.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Notifications.Accounts
{
    /// <summary>
    /// 存款交易已验证的领域通知
    /// </summary>
    public class DepositTransactionValidatedDomainNotification : SubTransactionDomainNotification<DepositTransactionId>
    {
        public DepositTransactionValidatedDomainNotification(DepositTransactionId transactionId, Money money)
        {
            TransactionId = transactionId;
            Money = money;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public DepositTransactionId TransactionId { get; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public Money Money { get; }
    }
}
