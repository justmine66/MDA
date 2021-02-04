using EBank.Domain.Models.Accounts;
using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Primitives;
using EBank.Domain.Models.Transferring;
using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Accounts
{
    /// <summary>
    /// 验证转账交易信息的领域命令
    /// </summary>
    public class ValidateTransferTransactionDomainCommand : DomainCommand<BankAccount, BankAccountId>
    {
        public TransferTransactionId TransactionId { get; set; }

        public Money Amount { get; set; }

        public TransferAccount Account { get; set; }
    }
}