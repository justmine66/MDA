namespace EBank.ApiServer.Application.Querying.Models
{
    public class BankAccountView
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

        /// <summary>
        /// 初始余额
        /// </summary>
        public decimal Balance { get; set; }
    }
}
