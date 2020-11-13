using MDA.Infrastructure.Serialization;
using Xunit;

namespace MDA.XUnitTest.Shared.Serialization
{
    public class BinarySerializerTester
    {
        [Fact]
        public void TestBssom()
        {
            var serializer = new BssomBinarySerializer();

            var source = new BankAccount();

            var bytes = serializer.Serialize(source);

            var sink = serializer.Deserialize<BankAccount>(bytes);

            Assert.Equal(sink, source);
        }
    }

    public interface IBankAccount
    {

    }

    /// <summary>
    /// 表示一个银行账户聚合根，封装状态。
    /// </summary>
    public class BankAccount : IBankAccount
    {
        /// <summary>
        /// 账户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public string Bank { get; set; }

        /// <summary>
        /// 可用余额。
        /// 计算规则：可用余额 = 账户余额 + 在途收入金额 - 在途支出金额。
        /// </summary>
        public decimal AvailableBalance { get; set; }
    }
}
