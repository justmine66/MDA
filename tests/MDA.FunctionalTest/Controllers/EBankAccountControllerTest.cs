using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using EBank.Application.Commanding.Accounts;
using Newtonsoft.Json;

namespace MDA.FunctionalTest.Controllers
{
    public class EBankAccountControllerTest : ControllerTestBase
    {
        public EBankAccountControllerTest(EBandApiTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task OpenAccountAsync()
        {
            var account = new OpenBankAccountApplicationCommand()
            {
                AccountName = "TestAccount1",
                Bank = "招商",
                InitialBalance = 1000
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/BankAccounts")
            {
                Content = new StringContent(JsonConvert.SerializeObject(account))
            };

            using var response = await Client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
