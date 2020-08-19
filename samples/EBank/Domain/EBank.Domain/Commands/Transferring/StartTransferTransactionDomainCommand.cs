using EBank.Domain.Models.Transferring;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Transferring
{
    /// <summary>
    /// 启动转账交易的领域命令
    /// </summary>
    public class StartTransferTransactionDomainCommand : DomainCommand<long>
    {
        /// <summary>
        /// 源账户信息
        /// </summary>
        public TransferTransactionAccount SourceAccount { get; set; }

        /// <summary>
        /// 目标账户信息
        /// </summary>
        public TransferTransactionAccount SinkAccount { get; set; }

        /// <summary>
        /// 转账金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
