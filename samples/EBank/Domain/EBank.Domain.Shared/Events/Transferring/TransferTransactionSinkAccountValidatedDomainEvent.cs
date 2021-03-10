using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Transferring
{
    /// <summary>
    /// 转账交易目标账户已验证的领域事件
    /// </summary>
    public class TransferTransactionSinkAccountValidatedDomainEvent : DomainEvent<TransferTransactionId> { }
}
