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
            var accountId = SnowflakeId.Default().NextId();
            var name = "justmine";
            var bank = "招商";

            // 开户
            var openAccountAppCommand = new OpenBankAccountApplicationCommand(name, bank, 1000);

            await _eBank.OpenAccountAsync(openAccountAppCommand, cancellationToken);

            // 存款
            var startDeposit = new StartDepositAccountTransactionApplicationCommand()
            {
                TransactionId = accountId,
                AccountId = accountId,
                AccountName = name,
                Bank = bank,
                Amount = 100
            };

            await _eBank.DepositedFundsAsync(startDeposit, cancellationToken);

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {

            await Task.CompletedTask;
        }
    }
}