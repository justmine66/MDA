using EBank.Application;
using EBank.Application.Commands.Accounts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EBank.ApiServer.Controllers
{
    [ApiController]
    [Route("api/v1/BankAccount")]
    public class BankAccountController : ControllerBase
    {
        private readonly IEBankApplicationService _eBank;

        public BankAccountController(IEBankApplicationService eBank)
        {
            _eBank = eBank;
        }

        [HttpPost]
        public async Task<IActionResult> Create(OpenBankAccountApplicationCommand command)
        {
            await _eBank.OpenAccountAsync(command);

            return Ok();
        }

        [Route("Deposit")]
        [HttpPost]
        public async Task<IActionResult> Deposit(StartDepositAccountTransactionApplicationCommand command)
        {
            await _eBank.DepositedFundsAsync(command);

            return Ok();
        }

        [Route("Withdraw")]
        [HttpPost]
        public async Task<IActionResult> Withdraw(StartWithdrawAccountTransactionApplicationCommand command)
        {
            await _eBank.WithdrawFundsAsync(command);

            return Ok();
        }
    }
}
