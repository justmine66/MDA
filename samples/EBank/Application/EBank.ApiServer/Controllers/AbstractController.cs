using Microsoft.AspNetCore.Mvc;

namespace EBank.ApiServer.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class AbstractController : ControllerBase { }
}
