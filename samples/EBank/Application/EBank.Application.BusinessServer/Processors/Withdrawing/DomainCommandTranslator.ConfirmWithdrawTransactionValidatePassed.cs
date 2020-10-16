using EBank.Application.Commands.Withdrawing;
using EBank.Domain.Commands.Withdrawing;
using MDA.Domain.Commands;
using System;

namespace EBank.Application.BusinessServer.Processors.Withdrawing
{
    public class ConfirmWithdrawTransactionValidatePassedDomainCommandTranslator :
        IDomainCommandFiller<ConfirmWithdrawTransactionValidatePassedDomainCommand, ConfirmWithdrawTransactionValidatePassedApplicationCommand>
    {
        private static readonly Lazy<ConfirmWithdrawTransactionValidatePassedDomainCommandTranslator> _instance = new Lazy<ConfirmWithdrawTransactionValidatePassedDomainCommandTranslator>(() => new ConfirmWithdrawTransactionValidatePassedDomainCommandTranslator());

        public static ConfirmWithdrawTransactionValidatePassedDomainCommandTranslator Instance => _instance.Value;

        public void Fill(ConfirmWithdrawTransactionValidatePassedDomainCommand domainCommand, ConfirmWithdrawTransactionValidatePassedApplicationCommand appCommand)
        {
            domainCommand.AggregateRootId = appCommand.TransactionId;
        }
    }
}
