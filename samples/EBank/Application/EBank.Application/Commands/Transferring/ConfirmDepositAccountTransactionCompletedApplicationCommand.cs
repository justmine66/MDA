﻿using MDA.Application.Commands;

namespace EBank.Application.Commands.Transferring
{
    /// <summary>
    /// 确认存款账户交易已完成的应用命令
    /// </summary>
    public class ConfirmDepositAccountTransactionCompletedApplicationCommand : ApplicationCommand<long>
    {
        /// <summary>
        /// 交易标识
        /// </summary>
        public long TransactionId { get; set; }

        /// <summary>
        /// 账户号
        /// </summary>
        public long AccountId { get; set; }
    }
}