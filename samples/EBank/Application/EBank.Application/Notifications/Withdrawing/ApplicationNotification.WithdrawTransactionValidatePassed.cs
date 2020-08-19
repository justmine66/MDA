using MDA.Application.Notifications;

namespace EBank.Application.Notifications.Withdrawing
{
    /// <summary>
    /// 取款交易信息验证成功的应用层通知
    /// </summary>
    public class WithdrawTransactionValidatePassedApplicationNotification : ApplicationNotification
    {
        public long TransactionId { get; set; }
    }
}
