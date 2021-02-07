using EBank.Domain.Models.Primitives;
using EBank.Domain.Models.Withdrawing.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Notifications.Accounts
{
    /// <summary>
    /// 取款交易验证失败的领域通知
    /// </summary>
    public class WithdrawTransactionValidatedDomainNotification : SubTransactionDomainNotification<WithdrawTransactionId>
    {
        public WithdrawTransactionValidatedDomainNotification(WithdrawTransactionId transactionId, Money money)
        {
            TransactionId = transactionId;
            Money = money;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public WithdrawTransactionId TransactionId { get; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public Money Money { get; }
    }
}
