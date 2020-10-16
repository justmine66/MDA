using EBank.Application.Commands.Withdrawing;
using EBank.Domain.Commands.Withdrawing;
using MDA.Domain.Commands;
using System;

namespace EBank.Application.BusinessServer.Processors.Withdrawing
{
    public class ConfirmWithdrawTransactionCompletedDomainCommandTranslator :
        IDomainCommandFiller<ConfirmWithdrawTransactionCompletedDomainCommand, ConfirmWithdrawTransactionCompletedApplicationCommand>
    {
        private static readonly Lazy<ConfirmWithdrawTransactionCompletedDomainCommandTranslator> _instance = new Lazy<ConfirmWithdrawTransactionCompletedDomainCommandTranslator>(() => new ConfirmWithdrawTransactionCompletedDomainCommandTranslator());

        public static ConfirmWithdrawTransactionCompletedDomainCommandTranslator Instance => _instance.Value;

        public void Fill(ConfirmWithdrawTransactionCompletedDomainCommand domainCommand,
            ConfirmWithdrawTransactionCompletedApplicationCommand appCommand)
        {
            domainCommand.AggregateRootId = appCommand.TransactionId;
        }
    }
}
