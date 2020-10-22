using EBank.Domain.Models.Accounts;
using EBank.Domain.Models.Transferring;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Accounts
{
    /// <summary>
    /// 验证转账交易信息的领域命令
    /// </summary>
    public class ValidateTransferTransactionDomainCommand : DomainCommand<BankAccount, long>
    {
        public long TransactionId { get; set; }

        public decimal Amount { get; set; }

        public TransferTransactionAccount Account { get; set; }

        public TransferTransactionAccountType AccountType { get; set; }
    }
}
