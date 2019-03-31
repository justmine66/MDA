using Basket.API.Model;
using Basket.API.Port.Adapters.Inbound;
using MDA.AspnetCore.Extensions;
using MDA.Concurrent;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync([FromBody] CustomerBasket basket, [FromHeader(Name = "x-requestid")] string requestId)
        {
            var disruptor = HttpContext.GetService<IInboundDisruptor<UpdateBasketInboundEvent>>();
            var result = await disruptor.PublishInboundEventAsync(new UpdateBasketEventTranslator(), requestId, basket);
            if (!result) return BadRequest();

            return Ok();
        }
    }
}
