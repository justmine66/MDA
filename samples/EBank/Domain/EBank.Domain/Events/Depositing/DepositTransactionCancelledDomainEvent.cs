using MDA.Domain.Events;

namespace EBank.Domain.Events.Depositing
{
    /// <summary>
    /// 存款交易已取消的领域事件
    /// </summary>
    public class DepositTransactionCancelledDomainEvent : DomainEvent<long> { }
}
