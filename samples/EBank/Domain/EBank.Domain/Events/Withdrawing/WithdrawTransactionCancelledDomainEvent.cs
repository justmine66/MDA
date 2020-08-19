using MDA.Domain.Events;

namespace EBank.Domain.Events.Withdrawing
{
    /// <summary>
    /// 取款交易已取消的领域事件
    /// </summary>
    public class WithdrawTransactionCancelledDomainEvent : DomainEvent<long> { }
}
