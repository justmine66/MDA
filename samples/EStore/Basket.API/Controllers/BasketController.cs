using Basket.API.Model;
using MDA.MessageBus;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Basket.API.IntegrationEvents;
using Basket.API.Services;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IMessageBus _messageBus;
        private readonly IIdentityService _identityService;
        private readonly IBasketRepository _repository;

        public BasketController(IMessageBus messageBus, IIdentityService identityService, IBasketRepository repository)
        {
            _messageBus = messageBus;
            _identityService = identityService;
            _repository = repository;
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
