using EBank.Application.Commands.Accounts;
using EBank.Domain.Commands.Accounts;
using MDA.Domain.Commands;
using System;

namespace EBank.Application.BusinessServer.CommandHandlers.Accounts
{
    public class StartWithdrawAccountTransactionDomainCommandTranslator :
        IDomainCommandFiller<StartWithdrawAccountTransactionDomainCommand, StartWithdrawAccountTransactionApplicationCommand>
    {
        private static readonly Lazy<StartWithdrawAccountTransactionDomainCommandTranslator> _instance = new Lazy<StartWithdrawAccountTransactionDomainCommandTranslator>(() => new StartWithdrawAccountTransactionDomainCommandTranslator());

        public static StartWithdrawAccountTransactionDomainCommandTranslator Instance => _instance.Value;

        public void TranslateTo(StartWithdrawAccountTransactionDomainCommand domainCommand, StartWithdrawAccountTransactionApplicationCommand appCommand)
        {
            domainCommand.AggregateRootId = appCommand.AccountId;
            domainCommand.AccountName = appCommand.AccountName;
            domainCommand.Bank = appCommand.Bank;
            domainCommand.Amount = appCommand.Amount;
            domainCommand.TransactionStage = appCommand.TransactionStage;
        }
    }
}
