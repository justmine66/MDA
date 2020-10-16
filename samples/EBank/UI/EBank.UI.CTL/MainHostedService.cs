using EBank.Application;
using EBank.Application.Commands.Accounts;
using MDA.Domain.Shared;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.UI.CTL
{
    public class MainHostedService : IHostedService
    {
        private readonly IEBankApplicationService _eBank;

        public MainHostedService(IEBankApplicationService eBank)
        {
            _eBank = eBank;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //var accountId = SnowflakeId.Default().NextId();

            //// 开户
            //var openAccountAppCommand = new OpenBankAccountApplicationCommand(accountId, "justmine", "招商", 1000);

            //_eBank.OpenAccount(openAccountAppCommand);

            // 存款
            var startDeposit = new StartDepositAccountTransactionApplicationCommand()
            {
                TransactionId = SnowflakeId.Default().NextId(),
                AccountId = 1317025891102437376,
                AccountName = "justmine",
                Bank = "招商",
                Amount = 100

            };

            _eBank.DepositedFunds(startDeposit);

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {

            await Task.CompletedTask;
        }
    }
}