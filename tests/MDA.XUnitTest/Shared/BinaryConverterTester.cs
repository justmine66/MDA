using MDA.Infrastructure.DataStructures;
using MDA.Infrastructure.Utils;
using System;
using Xunit;
using Xunit.Abstractions;

namespace MDA.XUnitTest.Shared
{
    public class BinaryConverterTester
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public BinaryConverterTester(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestBinaryConverterUtils()
        {
            try
            {
                var str = "1s!A";
                var str1 = "1s!Ab";
                var dt = DateTime.Now;
                var int0 = 1;
                short short0 = 1;
                long long0 = 1;

                var builder = new ByteBufferBuilder()
                    .AppendString(str)
                    .AppendString(str1)
                    .AppendDatetime(dt)
                    .AppendInt(int0)
                    .AppendShort(short0)
                    .AppendLong(long0);

                var buffer = builder.Build();

                Assert.Equal(BinarySerializationHelper.DeserializeString(buffer, 0, out var offset), str);
                Assert.Equal(BinarySerializationHelper.DeserializeString(buffer, offset, out offset), str1);
                Assert.Equal(BinarySerializationHelper.DeserializeDateTime(buffer, offset, out offset), dt);
                Assert.Equal(BinarySerializationHelper.DeserializeInt(buffer, offset, out offset), int0);
                Assert.Equal(BinarySerializationHelper.DeserializeShort(buffer, offset, out offset), short0);
                Assert.Equal(BinarySerializationHelper.DeserializeLong(buffer, offset, out offset), long0);

                var json = "{\"AccountName\":\"justmine\",\"Bank\":\"招商\",\"InitialBalance\":1000,\"Id\":0,\"AggregateRootId\":0,\"DomainCommandId\":\"fa434a0b700d429cb82ecdf511faf071\",\"DomainCommandType\":\"EBank.Domain.Commands.Accounts.OpenAccountDomainCommand, EBank.Domain, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = null\",\"DomainCommandVersion\":0,\"AggregateRootType\":\"EBank.Domain.Models.Accounts.BankAccount, EBank.Domain, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = null\",\"AggregateRootVersion\":0,\"Version\":1,\"Timestamp\":1602815582469,\"Topic\":\"Global\",\"PartitionKey\":0,\"Items\":{}}";
                var bytes = BinarySerializationHelper.SerializeString(json);
                var actualJson = BinarySerializationHelper.DeserializeString(bytes);

                Assert.Equal(json, actualJson);
            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine(e.ToString());
            }
        }
    }
}
