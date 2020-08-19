using EBank.Application.Commands.Depositing;
using EBank.Domain.Commands.Depositing;
using MDA.Domain.Commands;
using System;

namespace EBank.Application.BusinessServer.CommandHandlers.Depositing
{
    public class ConfirmDepositTransactionValidatePassedDomainCommandTranslator :
        IDomainCommandTranslator<ConfirmDepositTransactionValidatePassedDomainCommand, ConfirmDepositTransactionValidatePassedApplicationCommand>
    {
        private static readonly Lazy<ConfirmDepositTransactionValidatePassedDomainCommandTranslator> _instance = new Lazy<ConfirmDepositTransactionValidatePassedDomainCommandTranslator>(() => new ConfirmDepositTransactionValidatePassedDomainCommandTranslator());

        public static ConfirmDepositTransactionValidatePassedDomainCommandTranslator Instance => _instance.Value;

        public void TranslateTo(ConfirmDepositTransactionValidatePassedDomainCommand domainCommand, ConfirmDepositTransactionValidatePassedApplicationCommand appCommand)
        {
            domainCommand.AggregateRootId = appCommand.TransactionId;
        }
    }
}
