using System.ComponentModel.DataAnnotations;

namespace EBank.ApiServer.Models.Input.BankAccounts
{
    /// <summary>
    /// 重命名
    /// </summary>
    public class ChangeAccountName
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
    }
}
