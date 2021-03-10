using EBank.ApiServer.Infrastructure.ModelValidations;
using EBank.Domain.Models.Accounts;
using EBank.Domain.Models.Depositing;
using System.ComponentModel.DataAnnotations;

namespace EBank.ApiServer.Models.Input.Transactions
{
    /// <summary>
    /// 发起存款
    /// </summary>
    public class StartDepositInput
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

        /// <summary>
        /// 开户行
        /// </summary>
        /// <example>招商银行</example>
        [Required]
        [MinLength(BankAccount.Bank.Length.Minimum)]
        [MaxLength(BankAccount.Bank.Length.Maximum)]
        public string Bank { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        /// <example>100</example>
        [Required]
        [GreaterThan(DepositTransaction.Amount.Range.Minimum)]
        [LessThan(DepositTransaction.Amount.Range.Maximum)]
        public decimal Amount { get; set; }
    }
}
