using Basket.API.Model;
using Basket.API.Port.Adapters.Input;
using MDA.Concurrent;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IInboundDisruptor _disruptor;

        public BasketController(IInboundDisruptor disruptor)
        {
            _disruptor = disruptor;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync([FromBody] UpdateBasketInboundEvent inboundEvent, [FromHeader(Name = "x-requestid")] string requestId)
        {
            var result = await _disruptor.SendAsync(inboundEvent);
            if (!result)
                return BadRequest();

            return Ok();
        }
    }
}
