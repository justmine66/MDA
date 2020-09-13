using EBank.Application.Commands.Withdrawing;
using EBank.Domain.Commands.Withdrawing;
using MDA.Domain.Commands;
using System;

namespace EBank.Application.BusinessServer.CommandHandlers.Withdrawing
{
    public class CancelWithdrawTransactionDomainCommandTranslator :
        IDomainCommandFiller<CancelWithdrawTransactionDomainCommand, CancelWithdrawTransactionApplicationCommand>
    {
        private static readonly Lazy<CancelWithdrawTransactionDomainCommandTranslator> _instance = new Lazy<CancelWithdrawTransactionDomainCommandTranslator>(() => new CancelWithdrawTransactionDomainCommandTranslator());

        public static CancelWithdrawTransactionDomainCommandTranslator Instance => _instance.Value;

        public void Fill(CancelWithdrawTransactionDomainCommand domainCommand, CancelWithdrawTransactionApplicationCommand appCommand)
        {
            domainCommand.AggregateRootId = appCommand.TransactionId;
        }
    }
}
