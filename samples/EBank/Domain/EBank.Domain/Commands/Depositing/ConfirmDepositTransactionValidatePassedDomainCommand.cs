using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Depositing
{
    /// <summary>
    /// 确认存款交易信息验证通过的领域命令
    /// </summary>
    public class ConfirmDepositTransactionValidatePassedDomainCommand : DomainCommand<long> { }
}
