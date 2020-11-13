using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MDA.FunctionalTest.Controllers
{
    public class TransactionControllerTest : ControllerTestBase
    {
        public TransactionControllerTest(EBandApiTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task DepositAsync()
        {
            using var response = await Client.GetAsync("/api/v1/Transactions/Deposit");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task WithdrawAsync()
        {
            using var response = await Client.GetAsync("/api/v1/Transactions/Withdraw");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TransferAsync()
        {
            using var response = await Client.GetAsync("/api/v1/Transactions/Transfer");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
