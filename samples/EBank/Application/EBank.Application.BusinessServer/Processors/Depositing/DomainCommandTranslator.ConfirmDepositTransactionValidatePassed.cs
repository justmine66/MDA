using EBank.Application.Commands.Depositing;
using EBank.Domain.Commands.Depositing;
using MDA.Domain.Commands;
using System;

namespace EBank.Application.BusinessServer.Processors.Depositing
{
    public class ConfirmDepositTransactionValidatePassedDomainCommandTranslator :
        IDomainCommandFiller<ConfirmDepositTransactionValidatePassedDomainCommand, ConfirmDepositTransactionValidatePassedApplicationCommand>
    {
        private static readonly Lazy<ConfirmDepositTransactionValidatePassedDomainCommandTranslator> _instance = new Lazy<ConfirmDepositTransactionValidatePassedDomainCommandTranslator>(() => new ConfirmDepositTransactionValidatePassedDomainCommandTranslator());

        public static ConfirmDepositTransactionValidatePassedDomainCommandTranslator Instance => _instance.Value;

        public void Fill(ConfirmDepositTransactionValidatePassedDomainCommand domainCommand, ConfirmDepositTransactionValidatePassedApplicationCommand appCommand)
        {
            domainCommand.AggregateRootId = appCommand.TransactionId;
        }
    }
}
