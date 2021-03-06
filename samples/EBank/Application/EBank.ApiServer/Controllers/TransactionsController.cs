﻿using EBank.ApiServer.Application.Business;
using EBank.ApiServer.Application.Querying;
using EBank.ApiServer.Models.Input.Transactions;
using EBank.ApiServer.Models.Output;
using EBank.Application.Commands.Depositing;
using EBank.Application.Commands.Transferring;
using EBank.Application.Commands.Withdrawing;
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
        private readonly IBankAccountQueryService _accountQueryService;

        public TransactionsController(
            IEBankApplicationService eBankApplicationService,
            IBankAccountQueryService accountQueryService)
        {
            _eBankApplicationService = eBankApplicationService;
            _accountQueryService = accountQueryService;
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
        public async Task<IActionResult> DepositAsync(StartDepositInput dto)
        {
            if (!await _accountQueryService.HasAccountAsync(dto.AccountId))
            {
                return NotFound("The bank account does not exist.");
            }

            var command = ObjectPortMapper<StartDepositInput, StartDepositApplicationCommand>.Map(dto);

            await _eBankApplicationService.DepositedFundsAsync(command);

            return Accepted();
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
        public async Task<IActionResult> WithdrawAsync(StartWithdrawInput dto)
        {
            if (!await _accountQueryService.HasAccountAsync(dto.AccountId))
            {
                return NotFound("The bank account does not exist.");
            }

            var command = ObjectPortMapper<StartWithdrawInput, StartWithdrawApplicationCommand>.Map(dto);

            await _eBankApplicationService.WithdrawFundsAsync(command);

            return Accepted();
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
        public async Task<IActionResult> TransferAsync(StartTransferInput dto)
        {
            var source = dto.SourceAccount;
            if (!await _accountQueryService.HasAccountAsync(source.Id))
            {
                return NotFound("The source account does not exist.");
            }

            var sink = dto.SinkAccount;
            if (!await _accountQueryService.HasAccountAsync(sink.Id))
            {
                return NotFound("The sink account does not exist.");
            }

            var command = new StartTransferApplicationCommand()
            {
                Amount = dto.Amount,
                SourceAccount = ObjectPortMapper<StartTransferInput.TransferAccount, StartTransferApplicationCommand.TransferAccount>.Map(source),
                SinkAccount = ObjectPortMapper<StartTransferInput.TransferAccount, StartTransferApplicationCommand.TransferAccount>.Map(sink)
            };

            await _eBankApplicationService.TransferFundsAsync(command);

            return Accepted();
        }
    }
}
