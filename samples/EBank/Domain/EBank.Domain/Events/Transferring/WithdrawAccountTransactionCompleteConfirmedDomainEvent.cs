using EBank.Domain.Models.Transferring;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Transferring
{
    /// <summary>
    /// 从源账户完成扣款的账户交易已确认领域事件
    /// </summary>
    public class WithdrawAccountTransactionCompleteConfirmedDomainEvent : DomainEvent<long>
    {
        /// <summary>
        /// 交易标识
        /// </summary>
        public long TransactionId { get; set; }

        /// <summary>
        /// 目标账户
        /// </summary>
        public TransferTransactionAccount Sink{ get; set; }

        /// <summary>
        /// 存款金额
        /// </summary>
        public decimal DepositAmount { get; set; }
    }
}
