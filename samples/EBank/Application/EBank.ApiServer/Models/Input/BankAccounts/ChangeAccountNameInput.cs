using System.ComponentModel.DataAnnotations;
using EBank.ApiServer.Infrastructure.ModelValidations;
using EBank.Domain.Models.Accounts;

namespace EBank.ApiServer.Models.Input.BankAccounts
{
    /// <summary>
    /// 重命名
    /// </summary>
    public class ChangeAccountNameInput
    {
        /// <summary>
        /// 账户号
        /// </summary>
        /// <example>5392026437095184</example>
        [Required]
        [GreaterThanAndEqual(BankAccount.Id.Range.Minimum)]
        public long AccountId { get; set; }

        /// <summary>
        /// 账户名
        /// </summary>
        /// <example>张三</example>
        [Required]
        [MinLength(BankAccount.Name.Length.Minimum)]
        [MaxLength(BankAccount.Name.Length.Maximum)]
        public string AccountName { get; set; }
    }
}
