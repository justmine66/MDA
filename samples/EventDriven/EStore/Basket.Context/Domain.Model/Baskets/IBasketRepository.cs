namespace Basket.Context.Domain.Model.Baskets
{
    public interface IBasketRepository
    {
        BasketItemId GetNextIdentity();
    }
}
