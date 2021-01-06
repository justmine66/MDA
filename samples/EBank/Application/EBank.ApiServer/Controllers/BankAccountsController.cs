using EBank.ApiServer.Application.Business;
using EBank.ApiServer.Application.Querying;
using EBank.ApiServer.Models.Input;
using EBank.ApiServer.Models.Output;
using EBank.Application.Commanding.Accounts;
using MDA.Infrastructure.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using EBank.ApiServer.Models.Input.BankAccounts;

namespace EBank.ApiServer.Controllers
{
    /// <summary>
    /// 账户
    /// </summary>
    public class BankAccountsController : AbstractController
    {
        private readonly IEBankApplicationService _eBankApplicationService;
        private readonly IBankAccountQueryService _accountQueryService;

        public BankAccountsController(
            IEBankApplicationService eBankApplicationService,
            IBankAccountQueryService accountQueryService)
        {
            _eBankApplicationService = eBankApplicationService;
            _accountQueryService = accountQueryService;
        }

        /// <summary>
        /// 账户/开户
        /// </summary>
        /// <param name="dto">携带开户信息的数据传输对象</param>
        /// <response code="202">服务器已接受命令</response>
        /// <response code="400">表示请求不能被服务器理解，一般为客户端原因导致，比如：请参数验证失败。</response>
        /// <response code="401">身份认证失败</response>
        /// <response code="500">表示服务器发生了未知的异常。</response>
        /// <returns></returns>
        [HttpPost()]
#if !RELEASE
        [ProducesResponseType(typeof(ApiResult<long>), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiExceptionResult), StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
#endif
        public async Task<IActionResult> OpenAsync([FromBody] OpenBankAccount dto)
        {
            var command = ObjectPortMapper<OpenBankAccount, OpenBankAccountApplicationCommand>.Map(dto);

            await _eBankApplicationService.OpenAccountAsync(command);

            return Accepted(command.AccountId);
        }

        /// <summary>
        /// 账户/重命名
        /// </summary>
        /// <param name="dto">携带重命名信息的数据传输对象</param>
        /// <response code="202">服务器已接受命令</response>
        /// <response code="400">表示请求不能被服务器理解，一般为客户端原因导致，比如：请参数验证失败。</response>
        /// <response code="401">身份认证失败</response>
        /// <response code="500">表示服务器发生了未知的异常。</response>
        /// <returns></returns>
        [Route("rename")]
        [HttpPost]
#if !RELEASE
        [ProducesResponseType(typeof(ApiResult<long>), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiExceptionResult), StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
#endif
        public async Task<IActionResult> ChangeNameAsync(ChangeAccountName dto)
        {
            if (!await _accountQueryService.HasAccountAsync(dto.AccountId))
            {
                return NotFound();
            }

            var command = ObjectPortMapper<ChangeAccountName, ChangeAccountNameApplicationCommand>.Map(dto);

            await _eBankApplicationService.ChangeAccountNameAsync(command);

            return Accepted();
        }

        /// <summary>
        /// 账户/获取
        /// </summary>
        /// <param name="accountId">账号，比如：5392026437095184</param>
        /// <response code="200">成功。</response>
        /// <response code="400">表示请求不能被服务器理解，一般为客户端原因导致，比如：请参数验证失败。</response>
        /// <response code="401">身份认证失败</response>
        /// <response code="404">表示未能找到对应的数据，一般为条件不对。</response>
        /// <response code="500">表示服务器发生了未知的异常。</response>
        /// <returns></returns>
        [HttpGet("{accountId:long}")]
#if !RELEASE
        [ProducesResponseType(typeof(ApiResult<long>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiExceptionResult), StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
#endif
        public async Task<IActionResult> GetBankAccountAsync([Required, FromRoute] long accountId)
        {
            var account = await _accountQueryService.GetAccountAsync(accountId);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }
    }
}
