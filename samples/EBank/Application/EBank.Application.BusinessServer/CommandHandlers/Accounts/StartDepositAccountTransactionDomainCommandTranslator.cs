using EBank.Application.Commands.Accounts;
using EBank.Domain.Commands.Accounts;
using MDA.Domain.Commands;
using System;

namespace EBank.Application.BusinessServer.CommandHandlers.Accounts
{
    public class StartDepositAccountTransactionDomainCommandTranslator :
        IDomainCommandFiller<StartDepositAccountTransactionDomainCommand, StartDepositAccountTransactionApplicationCommand>
    {
        private static readonly Lazy<StartDepositAccountTransactionDomainCommandTranslator> _instance = new Lazy<StartDepositAccountTransactionDomainCommandTranslator>(() => new StartDepositAccountTransactionDomainCommandTranslator());

        public static StartDepositAccountTransactionDomainCommandTranslator Instance => _instance.Value;

        public void TranslateTo(StartDepositAccountTransactionDomainCommand domainCommand, StartDepositAccountTransactionApplicationCommand appCommand)
        {
            domainCommand.AggregateRootId = appCommand.AccountId;
            domainCommand.AccountName = appCommand.AccountName;
            domainCommand.Bank = appCommand.Bank;
            domainCommand.Amount = appCommand.Amount;
            domainCommand.TransactionStage = appCommand.TransactionStage;
        }
    }
}
