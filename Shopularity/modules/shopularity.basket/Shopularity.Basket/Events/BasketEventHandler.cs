using System.Linq;
using System.Threading.Tasks;
using Shopularity.Basket.Domain;
using Shopularity.Basket.Services;
using Shopularity.Ordering.Orders.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Shopularity.Basket.Events;

public class BasketEventHandler : IDistributedEventHandler<OrderCreatedEto>, ITransientDependency
{
    private readonly BasketManager _basketManager;

    public BasketEventHandler(BasketManager basketManager)
    {
        _basketManager = basketManager;
    }

    public async Task HandleEventAsync(OrderCreatedEto eventData)
    {
        await _basketManager.RemoveItemsFromUserBasketAsync(
            eventData.UserId,
            eventData.Products.Select(x => new BasketItem { Amount = x.Amount, ItemId = x.Product.Id }).ToList()
        );
    }
}