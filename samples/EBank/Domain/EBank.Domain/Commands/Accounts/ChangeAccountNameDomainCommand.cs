using EBank.Domain.Models.Accounts;
using EBank.Domain.Models.Accounts.Primitives;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Accounts
{
    public class ChangeAccountNameDomainCommand : DomainCommand<BankAccount, BankAccountId>
    {
        public ChangeAccountNameDomainCommand(BankAccountId accountId, BankAccountName accountName) 
            : base(accountId)
        {
            AccountName = accountName;
        }

        /// <summary>
        /// 账户名
        /// </summary>
        public BankAccountName AccountName { get; }
    }
}
