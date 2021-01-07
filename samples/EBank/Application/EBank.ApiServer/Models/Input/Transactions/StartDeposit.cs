using EBank.ApiServer.Infrastructure.ModelValidations;
using EBank.Domain.Models.Accounts;
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
        [GreaterThanAndEqual(DomainRules.PreConditions.Account.Id.Range.Minimum)]
        public long AccountId { get; set; }

        /// <summary>
        /// 账户名
        /// </summary>
        /// <example>张三</example>
        [Required]
        [MinLength(DomainRules.PreConditions.Account.Name.Length.Minimum)]
        [MaxLength(DomainRules.PreConditions.Account.Name.Length.Maximum)]
        public string AccountName { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        /// <example>招商银行</example>
        [Required]
        [MinLength(DomainRules.PreConditions.Account.Bank.Length.Minimum)]
        [MaxLength(DomainRules.PreConditions.Account.Bank.Length.Maximum)]
        public string Bank { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        /// <example>100</example>
        [Required]
        [GreaterThan(Domain.Models.Depositing.DomainRules.PreConditions.DepositTransaction.Amount.Range.Minimum)]
        [LessThan(Domain.Models.Depositing.DomainRules.PreConditions.DepositTransaction.Amount.Range.Maximum)]
        public decimal Amount { get; set; }
    }
}
