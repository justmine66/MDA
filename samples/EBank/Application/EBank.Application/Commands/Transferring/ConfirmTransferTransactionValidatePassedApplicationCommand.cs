using EBank.Domain.Models.Transferring;
using MDA.Application.Commands;

namespace EBank.Application.Commands.Transferring
{
    /// <summary>
    /// 确认转账交易信息验证通过的应用层命令
    /// </summary>
    public class ConfirmTransferTransactionValidatePassedApplicationCommand : ApplicationCommand<long>
    {
        public long TransactionId { get; set; }

        public TransferTransactionAccountType AccountType { get; set; }
    }
}
