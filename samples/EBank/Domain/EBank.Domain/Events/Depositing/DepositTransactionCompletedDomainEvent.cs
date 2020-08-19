using MDA.Domain.Events;

namespace EBank.Domain.Events.Depositing
{
    /// <summary>
    /// 存款交易已完成的领域事件
    /// </summary>
    public class DepositTransactionCompletedDomainEvent : DomainEvent<long> { }
}
