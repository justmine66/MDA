using EBank.Application;
using EBank.Application.Commands.Accounts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EBank.ApiServer.Controllers
{
    [ApiController]
    [Route("api/v1/BankAccount")]
    public class BankAccountsController : ControllerBase
    {
        private readonly IEBankApplicationService _eBank;

        public BankAccountsController(IEBankApplicationService eBank)
        {
            _eBank = eBank;
        }

        [HttpPost]
        public async Task<IActionResult> Open(OpenBankAccountApplicationCommand command)
        {
            await _eBank.OpenAccountAsync(command);

            return Ok();
        }
    }
}
