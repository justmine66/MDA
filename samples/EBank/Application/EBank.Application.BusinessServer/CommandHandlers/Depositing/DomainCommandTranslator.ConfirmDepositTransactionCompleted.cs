using EBank.Application.Commands.Depositing;
using EBank.Domain.Commands.Depositing;
using MDA.Domain.Commands;
using System;

namespace EBank.Application.BusinessServer.CommandHandlers.Depositing
{
    public class ConfirmDepositTransactionCompletedDomainCommandTranslator :
        IDomainCommandFiller<ConfirmDepositTransactionCompletedDomainCommand, ConfirmDepositTransactionCompletedApplicationCommand>
    {
        private static readonly Lazy<ConfirmDepositTransactionCompletedDomainCommandTranslator> _instance = new Lazy<ConfirmDepositTransactionCompletedDomainCommandTranslator>(() => new ConfirmDepositTransactionCompletedDomainCommandTranslator());

        public static ConfirmDepositTransactionCompletedDomainCommandTranslator Instance => _instance.Value;

        public void TranslateTo(ConfirmDepositTransactionCompletedDomainCommand domainCommand,
            ConfirmDepositTransactionCompletedApplicationCommand appCommand)
        {
            domainCommand.AggregateRootId = appCommand.TransactionId;
        }
    }
}
