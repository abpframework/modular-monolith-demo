using Shopularity.Basket.Domain;
using Shopularity.Basket.Services;
using Shopularity.Catalog.Products.Events;
using Shopularity.Ordering.Orders.Events;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace Shopularity.Events;

public class ShopularityEventHandler:
    IDistributedEventHandler<OrderCreatedEto>,
    IDistributedEventHandler<ProductsRequestCompletedEto>,
    IDistributedEventHandler<OrderLinesProcessedDto>
{
    private readonly IDistributedEventBus _eventBus;
    private readonly BasketManager _basketManager;

    public ShopularityEventHandler(IDistributedEventBus eventBus, BasketManager basketManager)
    {
        _eventBus = eventBus;
        _basketManager = basketManager;
    }
    
    public async Task HandleEventAsync(OrderCreatedEto eventData)
    {
        await _eventBus.PublishAsync(
                new ProductsRequestedEto
                {
                    Products = eventData.items.Select(x=> new KeyValuePair<Guid,int>(Guid.Parse(x.Key),x.Value)).ToDictionary()
                }
            );
    }

    public async Task HandleEventAsync(ProductsRequestCompletedEto eventData)
    {
        await _eventBus.PublishAsync(
            new OrderLinesReceivedEto
            {
                OrderId = Guid.Parse(eventData.RequesterId),
                Items = eventData.Products.Select(x=> 
                    new OrderItemDto
                    {
                        Name = x.Key.Name,
                        ItemId = x.Key.Id.ToString(),
                        Amount = x.Value,
                        Price = x.Key.Price
                    }).ToList()
            }
        );
    }

    public async Task HandleEventAsync(OrderLinesProcessedDto eventData)
    {
        await _basketManager.RemoveItemsFromUserBasketAsync(Guid.Parse(eventData.UserId), eventData.Items.Select(x => new BasketItem
        {
            ItemId = x.ItemId,
            Amount = x.Amount
        }).ToList());
    }
}