using Basket.API.Model;
using Basket.API.Port.Adapters.Inbound;
using MDA.Concurrent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IInboundDisruptor<UpdateBasketInboundEvent> _disruptor;

        public BasketController(ILogger<BasketController> logger, IInboundDisruptor<UpdateBasketInboundEvent> disruptor)
        {
            _disruptor = disruptor;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync([FromBody] CustomerBasket basket, [FromHeader(Name = "x-requestid")] string requestId)
        {
            var result = await _disruptor.PublishInboundEventAsync(new UpdateBasketEventTranslator(), requestId, basket);
            if (!result) return BadRequest();

            return Ok();
        }
    }
}
