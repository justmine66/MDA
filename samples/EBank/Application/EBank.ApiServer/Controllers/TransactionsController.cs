using EBank.ApiServer.Application.Business;
using EBank.ApiServer.Models.Input.Transactions;
using EBank.ApiServer.Models.Output;
using EBank.Application.Commanding.Depositing;
using EBank.Application.Commanding.Transferring;
using EBank.Application.Commanding.Withdrawing;
using MDA.Infrastructure.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;

namespace EBank.ApiServer.Controllers
{
    /// <summary>
    /// 交易
    /// </summary>
    public class TransactionsController : AbstractController
    {
        private readonly IEBankApplicationService _eBankApplicationService;

        public TransactionsController(IEBankApplicationService eBankApplicationService)
        {
            _eBankApplicationService = eBankApplicationService;
        }

        /// <summary>
        /// 交易/存款
        /// </summary>
        /// <param name="dto">携带存款信息的数据传输对象</param>
        /// <response code="202">服务器已接受命令</response>
        /// <response code="400">表示请求不能被服务器理解，一般为客户端原因导致，比如：请参数验证失败。</response>
        /// <response code="401">身份认证失败</response>
        /// <response code="500">表示服务器发生了未知的异常。</response>
        /// <returns></returns>
        [Route("Deposit")]
        [HttpPost]
#if !RELEASE
        [ProducesResponseType(typeof(ApiResult<long>), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiExceptionResult), StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
#endif
        public async Task<IActionResult> DepositAsync(StartDeposit dto)
        {
            var command = ObjectPortMapper<StartDeposit, StartDepositApplicationCommand>.Map(dto);

            await _eBankApplicationService.DepositedFundsAsync(command);

            return Ok();
        }

        /// <summary>
        /// 交易/取款
        /// </summary>
        /// <param name="dto">携带取款信息的数据传输对象</param>
        /// <response code="202">服务器已接受命令</response>
        /// <response code="400">表示请求不能被服务器理解，一般为客户端原因导致，比如：请参数验证失败。</response>
        /// <response code="401">身份认证失败</response>
        /// <response code="500">表示服务器发生了未知的异常。</response>
        /// <returns></returns>
        [Route("Withdraw")]
        [HttpPost]
#if !RELEASE
        [ProducesResponseType(typeof(ApiResult<long>), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiExceptionResult), StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
#endif
        public async Task<IActionResult> WithdrawAsync(StartWithdraw dto)
        {
            var command = ObjectPortMapper<StartWithdraw, StartWithdrawApplicationCommand>.Map(dto);

            await _eBankApplicationService.WithdrawFundsAsync(command);

            return Ok();
        }

        /// <summary>
        /// 交易/转账
        /// </summary>
        /// <param name="dto">携带转账信息的数据传输对象</param>
        /// <response code="202">服务器已接受命令</response>
        /// <response code="400">表示请求不能被服务器理解，一般为客户端原因导致，比如：请参数验证失败。</response>
        /// <response code="401">身份认证失败</response>
        /// <response code="500">表示服务器发生了未知的异常。</response>
        /// <returns></returns>
        [Route("Transfer")]
        [HttpPost]
#if !RELEASE
        [ProducesResponseType(typeof(ApiResult<long>), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiExceptionResult), StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
#endif
        public async Task<IActionResult> WithdrawAsync(StartTransfer dto)
        {
            var command = ObjectPortMapper<StartTransfer, StartTransferApplicationCommand>.Map(dto);

            await _eBankApplicationService.TransferFundsAsync(command);

            return Ok();
        }
    }
}
