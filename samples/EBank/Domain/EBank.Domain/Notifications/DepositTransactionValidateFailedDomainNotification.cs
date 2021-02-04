using EBank.Domain.Models.Depositing.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Notifications
{
    /// <summary>
    /// 存款交易验证失败的领域通知
    /// </summary>
    public class DepositTransactionValidateFailedDomainNotification : EndSubTransactionDomainNotification<DepositTransactionId>
    {
        public DepositTransactionValidateFailedDomainNotification(DepositTransactionId transactionId, string reason)
        {
            TransactionId = transactionId;
            Message = reason;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public DepositTransactionId TransactionId { get; }
    }
}
