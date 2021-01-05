using System.ComponentModel.DataAnnotations;

namespace EBank.ApiServer.Models.Input.BankAccounts
{
    /// <summary>
    /// 开户
    /// </summary>
    public class OpenBankAccount
    {
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
        /// 初始余额
        /// </summary>
        /// <example>1000</example>
        public decimal? InitialBalance { get; set; }
    }
}
