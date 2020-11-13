using MDA.Infrastructure.Serialization;
using Xunit;

namespace MDA.XUnitTest.Shared.Serialization
{
    public class JsonSerializerTester
    {
        [Fact]
        public void TestBssom()
        {
            var serializer = new KoobooJsonSerializer();

            IBankAccount source = new BankAccount();

            var json = serializer.Serialize(source);

            var sink = serializer.Deserialize<BankAccount>(json);

            Assert.Equal(sink, source);
        }
    }
}
