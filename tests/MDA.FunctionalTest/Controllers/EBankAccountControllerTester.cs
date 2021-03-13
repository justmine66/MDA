using EBank.ApiServer.Models.Output.BankAccounts;
using EBank.Application.Commands.Accounts;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MDA.FunctionalTest.Controllers
{
    public class EBankAccountControllerTester : ControllerTestBase
    {
        public EBankAccountControllerTester(EBandApiTestFixture fixture)
            : base(fixture) { }

        [Fact]
        public async Task OpenAccountAsync()
        {
            var command = new OpenBankAccountApplicationCommand()
            {
                AccountName = "TestAccount1",
                Bank = "招商",
                InitialBalance = 1000
            };

            using var response = await Client.PostAsync("api/v1/BankAccounts", command);

            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

            while (true)
            {
                var account = await Client.GetAsync<GetBankAccountOutput>($"api/v1/BankAccounts?AccountId={command.AccountId}");
                if (account == null)
                {
                    await Task.Delay(TimeSpan.FromSeconds(2));

                    continue;
                }

                Assert.Equal(command.AccountId, account.Id);
                Assert.Equal(command.AccountName, account.Name);
                Assert.Equal(command.Bank, account.Bank);
                Assert.Equal(command.InitialBalance, account.Balance);

                break;
            }
        }
    }
}
