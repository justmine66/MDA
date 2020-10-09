using EBank.Application;
using EBank.Application.Commands.Accounts;
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
            var openAccountAppCommand = new OpenBankAccountApplicationCommand(1, "justmine", "招商", 1000);

            _eBank.OpenAccount(openAccountAppCommand);

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {

            await Task.CompletedTask;
        }
    }
}