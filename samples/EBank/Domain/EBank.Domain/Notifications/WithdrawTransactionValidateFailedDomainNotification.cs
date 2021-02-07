using EBank.Domain.Models.Withdrawing.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Notifications
{
    /// <summary>
    /// 取款交易验证失败的领域通知
    /// </summary>
    public class WithdrawTransactionValidateFailedDomainNotification : EndSubTransactionDomainNotification<WithdrawTransactionId>
    {
        public WithdrawTransactionValidateFailedDomainNotification(WithdrawTransactionId transactionId, string message)
        {
            TransactionId = transactionId;
            Message = message;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public WithdrawTransactionId TransactionId { get; }
    }
}
