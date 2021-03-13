using MDA.Application.Commands;
using System.ComponentModel.DataAnnotations;

namespace EBank.Application.Commands.Accounts
{
    /// <summary>
    /// 变更账户名的应用命令
    /// </summary>
    public class ChangeAccountNameApplicationCommand : ApplicationCommand
    {
        /// <summary>
        /// 账户号
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// 账户名
        /// </summary>
        [Required]
        public string AccountName { get; set; }
    }
}
