﻿using EBank.Domain.Models.Depositing;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Depositing
{
    /// <summary>
    /// 存款交易已完成的领域事件
    /// </summary>
    public class DepositTransactionCompletedDomainEvent : DomainEvent<long>
    {
        public DepositTransactionCompletedDomainEvent(DepositTransactionStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public DepositTransactionStatus Status{ get;  }
}
}
