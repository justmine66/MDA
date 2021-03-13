using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Transferring
{
    /// <summary>
    /// 转账交易源账户已验证的领域事件
    /// </summary>
    public class TransferTransactionSourceAccountValidatedDomainEvent : DomainEvent<TransferTransactionId> { }
}
