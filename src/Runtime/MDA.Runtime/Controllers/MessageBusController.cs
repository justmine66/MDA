using MDA.Runtime.BuildBlocks.MessageBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MDA.Runtime.Controllers
{
    [ApiController]
    [Route("v1.0")]
    public class MessageBusController : ControllerBase
    {
        private readonly ILogger<MessageBusController> _logger;

        public MessageBusController(ILogger<MessageBusController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("/publish")]
        public Task<PublishResponse> PublishAsync(PublishRequest request)
        {
            
        }
    }
}
