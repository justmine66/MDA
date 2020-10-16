using EBank.Application.Commands.Accounts;
using EBank.Domain.Commands.Accounts;
using MDA.Domain.Commands;
using System;

namespace EBank.Application.BusinessServer.Processors.Accounts
{
    public class StartDepositAccountTransactionDomainCommandFiller :
        IDomainCommandFiller<StartDepositAccountTransactionDomainCommand, StartDepositAccountTransactionApplicationCommand>
    {
        private static readonly Lazy<StartDepositAccountTransactionDomainCommandFiller> _instance = new Lazy<StartDepositAccountTransactionDomainCommandFiller>(() => new StartDepositAccountTransactionDomainCommandFiller());

        public static StartDepositAccountTransactionDomainCommandFiller Instance => _instance.Value;

        public void Fill(StartDepositAccountTransactionDomainCommand domainCommand, StartDepositAccountTransactionApplicationCommand appCommand)
        {
            domainCommand.AggregateRootId = appCommand.AccountId;
            domainCommand.AccountName = appCommand.AccountName;
            domainCommand.Bank = appCommand.Bank;
            domainCommand.Amount = appCommand.Amount;
            domainCommand.TransactionStage = appCommand.TransactionStage;
        }
    }
}
