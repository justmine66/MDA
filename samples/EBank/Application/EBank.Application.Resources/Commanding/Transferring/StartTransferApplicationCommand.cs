using MDA.Application.Commands;

namespace EBank.Application.Resources.Commanding.Transferring
{
    /// <summary>
    /// 发起转账的应用命令
    /// </summary>
    public class StartTransferApplicationCommand : ApplicationCommand
    {
        /// <summary>
        /// 源账户
        /// </summary>
        public TransferAccount SourceAccount { get; set; }

        /// <summary>
        /// 目标账户
        /// </summary>
        public TransferAccount SinkAccount { get; set; }

        /// <summary>
        /// 转账金额
        /// </summary>
        public decimal Amount { get; set; }
    }

    public class TransferAccount
    {
        /// <summary>
        /// 账户号
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 账户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public string Bank { get; set; }
    }
}
