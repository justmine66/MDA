using EBank.Domain.Models.Transferring;
using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Notifications
{
    /// <summary>
    /// 转账交易验证失败的领域通知
    /// </summary>
    public class TransferTransactionValidateFailedDomainNotification : EndSubTransactionDomainNotification<TransferTransactionId>
    {
        public TransferTransactionValidateFailedDomainNotification(TransferTransactionId transactionId, TransferAccountType accountType, string message)
        {
            TransactionId = transactionId;
            AccountType = accountType;
            Message = message;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public TransferTransactionId TransactionId { get; }

        /// <summary>
        /// 账户类型
        /// </summary>
        public TransferAccountType AccountType { get; }
    }
}
