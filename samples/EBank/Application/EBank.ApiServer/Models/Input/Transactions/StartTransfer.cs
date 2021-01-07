using System.ComponentModel.DataAnnotations;

namespace EBank.ApiServer.Models.Input.Transactions
{
    /// <summary>
    /// 发起转账
    /// </summary>
    public class StartTransfer
    {
        /// <summary>
        /// 源账户
        /// </summary>
        [Required]
        public TransferAccount SourceAccount { get; set; }

        /// <summary>
        /// 目标账户
        /// </summary>
        [Required]
        public TransferAccount SinkAccount { get; set; }

        /// <summary>
        /// 转账金额
        /// </summary>
        [Required]
        public decimal Amount { get; set; }

        /// <summary>
        /// 转账账号
        /// </summary>
        public class TransferAccount
        {
            /// <summary>
            /// 账户号
            /// </summary>
            /// <example>5392026437095184</example>
            [Required]
            public long Id { get; set; }

            /// <summary>
            /// 账户名
            /// </summary>
            /// <example>张三</example>
            [Required]
            public string Name { get; set; }

            /// <summary>
            /// 开户行
            /// </summary>
            /// <example>100</example>
            [Required]
            public string Bank { get; set; }
        }
    }
}
