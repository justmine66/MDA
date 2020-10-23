using EBank.Application;
using EBank.Application.Commands.Depositing;
using EBank.Application.Commands.Transferring;
using EBank.Application.Commands.Withdrawing;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EBank.ApiServer.Controllers
{
    [ApiController]
    [Route("api/v1/Transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly IEBankApplicationService _eBank;

        public TransactionsController(IEBankApplicationService eBank)
        {
            _eBank = eBank;
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

        [Route("Transfer")]
        [HttpPost]
        public async Task<IActionResult> Withdraw(StartTransferApplicationCommand command)
        {
            await _eBank.TransferFundsAsync(command);

            return Ok();
        }
    }
}
