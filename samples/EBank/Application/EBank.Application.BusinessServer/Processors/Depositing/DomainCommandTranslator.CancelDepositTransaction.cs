using EBank.Application.Commands.Depositing;
using EBank.Domain.Commands.Depositing;
using MDA.Domain.Commands;
using System;

namespace EBank.Application.BusinessServer.Processors.Depositing
{
    public class CancelDepositTransactionDomainCommandTranslator :
        IDomainCommandFiller<CancelDepositTransactionDomainCommand, CancelDepositTransactionApplicationCommand>
    {
        private static readonly Lazy<CancelDepositTransactionDomainCommandTranslator> _instance = new Lazy<CancelDepositTransactionDomainCommandTranslator>(() => new CancelDepositTransactionDomainCommandTranslator());

        public static CancelDepositTransactionDomainCommandTranslator Instance => _instance.Value;

        public void Fill(CancelDepositTransactionDomainCommand domainCommand, CancelDepositTransactionApplicationCommand appCommand)
        {
            domainCommand.AggregateRootId = appCommand.TransactionId;
        }
    }
}
