using EBank.ApiServer.Application.Business;
using EBank.ApiServer.Application.Querying;
using EBank.Application.Commanding.Accounts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EBank.ApiServer.Controllers
{
    /// <summary>
    /// 银行账户控制器
    /// </summary>
    [ApiController]
    [Route("api/v1/BankAccounts")]
    public class BankAccountsController : ControllerBase
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
        /// 开户
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns></returns>
        /// <response code="201">服务器已接受命令</response>
        /// <response code="400">请求参数不合法</response>
        /// <response code="401">身份认证失败</response>
        [HttpPost]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> OpenAsync([FromBody] OpenBankAccountApplicationCommand command)
        {
            await _eBankApplicationService.OpenAccountAsync(command);

            return Accepted(command.AccountId);
        }

        /// <summary>
        /// 变更账户名
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns></returns>
        [Route("ChangeName")]
        [HttpPost]
        public async Task<IActionResult> ChangeNameAsync(ChangeAccountNameApplicationCommand command)
        {
            await _eBankApplicationService.ChangeAccountNameAsync(command);

            return Ok();
        }

        /// <summary>
        /// 获取银行账户
        /// </summary>
        /// <param name="accountId">账号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBankAccountAsync(long accountId)
        {
            var account = await _accountQueryService.GetAccountAsync(accountId);

            return Ok(account);
        }
    }
}
