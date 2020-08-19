using MDA.Domain.Events;

namespace EBank.Domain.Events.Withdrawing
{
    /// <summary>
    /// 取款交易已完成的领域事件
    /// </summary>
    public class WithdrawTransactionCompletedDomainEvent : DomainEvent<long> { }
}
