using EBank.ApiServer.Infrastructure.ModelValidations;
using EBank.Domain.Models.Accounts;
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
        [MinLength(BankAccount.Name.Length.Minimum)]
        [MaxLength(BankAccount.Name.Length.Maximum)]
        public string AccountName { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        /// <example>招商银行</example>
        [Required]
        [MinLength(BankAccount.Bank.Length.Minimum)]
        [MaxLength(BankAccount.Bank.Length.Maximum)]
        public string Bank { get; set; }

        /// <summary>
        /// 初始余额
        /// </summary>
        /// <example>1000</example>
        [GreaterThanAndEqual(BankAccount.InitialBalance.Range.Minimum)]
        public decimal InitialBalance { get; set; }
    }
}
