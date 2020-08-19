using EBank.Domain.Models.Transferring;
using MDA.Application.Commands;

namespace EBank.Application.Commands.Transferring
{
    /// <summary>
    /// 验证转账交易信息的应用层命令
    /// </summary>
    public class ValidateTransferTransactionApplicationCommand : ApplicationCommand<long>
    {
        public long TransactionId { get; set; }

        public TransferTransactionAccount Account { get; set; }

        public TransferTransactionAccountType AccountType { get; set; }
    }
}
