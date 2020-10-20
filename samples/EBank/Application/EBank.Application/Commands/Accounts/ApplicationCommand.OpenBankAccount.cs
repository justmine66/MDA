﻿using MDA.Application.Commands;
using MDA.Domain.Shared;

namespace EBank.Application.Commands.Accounts
{
    public class OpenBankAccountApplicationCommand : ApplicationCommand
    {
        /// <summary>
        /// 账户号
        /// </summary>
        public long AccountId { get; set; } = SnowflakeId.Default().NextId();

        /// <summary>
        /// 账户名
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public string Bank { get; set; }

        /// <summary>
        /// 初始余额
        /// </summary>
        public decimal InitialBalance { get; set; }
    }
}
