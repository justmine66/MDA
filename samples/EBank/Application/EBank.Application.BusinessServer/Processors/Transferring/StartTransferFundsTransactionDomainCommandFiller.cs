using EBank.Application.Commands.Transferring;
using EBank.Domain.Commands.Transferring;
using MDA.Domain.Commands;
using System;

namespace EBank.Application.BusinessServer.Processors.Transferring
{
    public class ConfirmDepositTransactionValidatePassedDomainCommandTranslator :
        IDomainCommandFiller<StartTransferTransactionDomainCommand, TransferFundsApplicationCommand>
    {
        public static readonly Lazy<ConfirmDepositTransactionValidatePassedDomainCommandTranslator> _instance = new Lazy<ConfirmDepositTransactionValidatePassedDomainCommandTranslator>(() => new ConfirmDepositTransactionValidatePassedDomainCommandTranslator());

        public static ConfirmDepositTransactionValidatePassedDomainCommandTranslator Instance => _instance.Value;

        public void Fill(StartTransferTransactionDomainCommand domainCommand, TransferFundsApplicationCommand appCommand)
        {
            domainCommand.SourceAccount = appCommand.SourceAccount;
            domainCommand.SinkAccount = appCommand.SinkAccount;
            domainCommand.Amount = appCommand.Amount;
        }
    }
}
