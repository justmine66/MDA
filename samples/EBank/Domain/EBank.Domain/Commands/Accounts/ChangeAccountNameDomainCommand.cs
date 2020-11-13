using EBank.Domain.Models.Accounts;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Accounts
{
    public class ChangeAccountNameDomainCommand : DomainCommand<BankAccount, long>
    {
        public ChangeAccountNameDomainCommand(long accountId, string accountName) 
            : base(accountId)
        {
            AccountName = accountName;
        }

        /// <summary>
        /// 账户名
        /// </summary>
        public string AccountName { get; }
    }
}
