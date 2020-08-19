using EBank.Domain.Models.Transferring;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Transferring
{
    public class ConfirmTransferTransactionAccountValidatePassedDomainCommand : DomainCommand<long>
    {
        public TransferTransactionAccountType AccountType { get; set; }
    }
}
