using MDA.Application.Notifications;

namespace EBank.Application.Notifications.Depositing
{
    /// <summary>
    /// 存款交易信息验证成功的应用层通知
    /// </summary>
    public class DepositTransactionValidatePassedApplicationNotification : ApplicationNotification
    {
        public long TransactionId { get; set; }
    }
}
