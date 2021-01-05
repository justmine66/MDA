using System.ComponentModel.DataAnnotations;

namespace EBank.ApiServer.Models.Input.Transactions
{
    /// <summary>
    /// 发起存款
    /// </summary>
    public class StartDeposit
    {
        /// <summary>
        /// 账户号
        /// </summary>
        /// <example>5392026437095184</example>
        [Required]
        public long AccountId { get; set; }

        /// <summary>
        /// 账户名
        /// </summary>
        /// <example>张三</example>
        [Required]
        public string AccountName { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        /// <example>招商银行</example>
        [Required]
        public string Bank { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        /// <example>1000</example>
        [Required]
        public decimal Amount { get; set; }
    }
}
