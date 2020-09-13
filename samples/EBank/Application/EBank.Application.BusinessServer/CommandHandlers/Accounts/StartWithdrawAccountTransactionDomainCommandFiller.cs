using EBank.Application.Commands.Accounts;
using EBank.Domain.Commands.Accounts;
using MDA.Domain.Commands;
using System;

namespace EBank.Application.BusinessServer.CommandHandlers.Accounts
{
    public class StartWithdrawAccountTransactionDomainCommandFiller :
        IDomainCommandFiller<StartWithdrawAccountTransactionDomainCommand, StartWithdrawAccountTransactionApplicationCommand>
    {
        private static readonly Lazy<StartWithdrawAccountTransactionDomainCommandFiller> _instance = new Lazy<StartWithdrawAccountTransactionDomainCommandFiller>(() => new StartWithdrawAccountTransactionDomainCommandFiller());

        public static StartWithdrawAccountTransactionDomainCommandFiller Instance => _instance.Value;

        public void Fill(StartWithdrawAccountTransactionDomainCommand domainCommand, StartWithdrawAccountTransactionApplicationCommand appCommand)
        {
            domainCommand.AggregateRootId = appCommand.AccountId;
            domainCommand.AccountName = appCommand.AccountName;
            domainCommand.Bank = appCommand.Bank;
            domainCommand.Amount = appCommand.Amount;
            domainCommand.TransactionStage = appCommand.TransactionStage;
        }
    }
}
