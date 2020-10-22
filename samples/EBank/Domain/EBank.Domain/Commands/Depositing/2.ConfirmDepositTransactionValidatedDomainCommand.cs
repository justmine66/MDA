using EBank.Domain.Models.Depositing;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Depositing
{
    /// <summary>
    /// 确认存款交易信息已验证的领域命令
    /// </summary>
    public class ConfirmDepositTransactionValidatedDomainCommand : DomainCommand<DepositTransaction, long> { }
}
