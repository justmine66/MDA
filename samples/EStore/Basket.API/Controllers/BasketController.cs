using Basket.API.IntegrationEvents;
using Basket.API.Model;
using Basket.API.Services;
using MDA.Concurrent;
using MDA.MessageBus;
using MDA.Persistence;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IMessageBus _messageBus;
        private readonly IIdentityService _identityService;
        private readonly IBasketRepository _repository;
        private readonly IInboundDisruptor<UpdateBasketEvent> _disruptor;
        private readonly IJournalable _journaler;

        public BasketController(IMessageBus messageBus, IIdentityService identityService, IBasketRepository repository, IJournalable journaler)
        {
            _messageBus = messageBus;
            _identityService = identityService;
            _repository = repository;
            _journaler = journaler;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync([FromBody] CustomerBasket value)
        {
            var journal = new UpdateBasketJournaler(_journaler);
            var logicProcessor = new UpdateBasketJournaler(_journaler);

            _disruptor.After(journal).HandleEventsWith(logicProcessor);

            return Ok();
        }

        [Route("checkout")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CheckoutAsync(
            [FromBody]BasketCheckout basketCheckout,
            [FromHeader(Name = "x-requestid")] string requestId)
        {
            basketCheckout.RequestId = (Guid.TryParse(requestId, out var guid) && guid != Guid.Empty) ?
                guid : basketCheckout.RequestId;

            var userId = _identityService.GetUserIdentity();
            var basket = await _repository.GetBasketAsync(userId);
            if (basket == null)
            {
                return BadRequest();
            }

            var userName = User.FindFirst(x => x.Type == "unique_name").Value;

            var message = new UserCheckoutAcceptedIntegrationEvent(userId, userName, basketCheckout.City, basketCheckout.Street,
                basketCheckout.State, basketCheckout.Country, basketCheckout.ZipCode, basketCheckout.CardNumber, basketCheckout.CardHolderName,
                basketCheckout.CardExpiration, basketCheckout.CardSecurityNumber, basketCheckout.CardTypeId, basketCheckout.Buyer, basketCheckout.RequestId, basket);

            await _messageBus.PublishAsync(message);

            return Accepted();
        }
    }
}
