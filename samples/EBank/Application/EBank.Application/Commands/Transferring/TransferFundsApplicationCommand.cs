using EBank.Domain.Models.Transferring;
using MDA.Application.Commands;

namespace EBank.Application.Commands.Transferring
{
    public class TransferFundsApplicationCommand : ApplicationCommand
    {
        /// <summary>
        /// 源账户
        /// </summary>
        public TransferTransactionAccount SourceAccount { get; set; }

        /// <summary>
        /// 目标账户
        /// </summary>
        public TransferTransactionAccount SinkAccount { get; set; }

        /// <summary>
        /// 转账金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
