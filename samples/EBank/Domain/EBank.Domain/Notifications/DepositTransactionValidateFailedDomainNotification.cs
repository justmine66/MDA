using MDA.Domain.Saga;

namespace EBank.Domain.Notifications
{
    /// <summary>
    /// 存款交易验证失败的领域通知
    /// </summary>
    public class DepositTransactionValidateFailedDomainNotification : EndSubTransactionDomainNotification<long>
    {
        public DepositTransactionValidateFailedDomainNotification(long transactionId, string reason)
        {
            TransactionId = transactionId;
            Message = reason;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public long TransactionId { get; }
    }
}
