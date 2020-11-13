using EBank.ApiServer.Application.Business;
using EBank.Application.Commanding.Depositing;
using EBank.Application.Commanding.Transferring;
using EBank.Application.Commanding.Withdrawing;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EBank.ApiServer.Controllers
{
    /// <summary>
    /// 交易控制器
    /// </summary>
    [ApiController]
    [Route("api/v1/Transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly IEBankApplicationService _eBank;

        public TransactionsController(IEBankApplicationService eBank)
        {
            _eBank = eBank;
        }

        /// <summary>
        /// 存款
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns></returns>
        [Route("Deposit")]
        [HttpPost]
        public async Task<IActionResult> DepositAsync(StartDepositApplicationCommand command)
        {
            await _eBank.DepositedFundsAsync(command);

            return Ok();
        }

        /// <summary>
        /// 取款
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns></returns>
        [Route("Withdraw")]
        [HttpPost]
        public async Task<IActionResult> WithdrawAsync(StartWithdrawApplicationCommand command)
        {
            await _eBank.WithdrawFundsAsync(command);

            return Ok();
        }

        /// <summary>
        /// 转账
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns></returns>
        [Route("Transfer")]
        [HttpPost]
        public async Task<IActionResult> WithdrawAsync(StartTransferApplicationCommand command)
        {
            await _eBank.TransferFundsAsync(command);

            return Ok();
        }
    }
}
