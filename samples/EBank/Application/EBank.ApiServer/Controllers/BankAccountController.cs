using EBank.Application;
using EBank.Application.Commands.Accounts;
using EBank.Application.Commands.Depositing;
using EBank.Application.Commands.Withdrawing;
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
        public async Task<IActionResult> Deposit(StartDepositApplicationCommand command)
        {
            await _eBank.DepositedFundsAsync(command);

            return Ok();
        }

        [Route("Withdraw")]
        [HttpPost]
        public async Task<IActionResult> Withdraw(StartWithdrawApplicationCommand command)
        {
            await _eBank.WithdrawFundsAsync(command);

            return Ok();
        }
    }
}
