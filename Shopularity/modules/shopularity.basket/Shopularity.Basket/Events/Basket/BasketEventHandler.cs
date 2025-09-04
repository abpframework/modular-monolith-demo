using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Shopularity.Basket.Domain.Basket;
using Shopularity.Basket.SignalR;
using Shopularity.Ordering.Events.Orders;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;

namespace Shopularity.Basket.Events.Basket;

public class BasketEventHandler : 
    IDistributedEventHandler<OrderCreatedEto>,
    ILocalEventHandler<BasketUpdatedEto>,
    ITransientDependency
{
    private readonly BasketManager _basketManager;
    private readonly IHubContext<BasketHub> _basketHub;

    public BasketEventHandler(
        BasketManager basketManager,
        IHubContext<BasketHub> basketHub)
    {
        _basketManager = basketManager;
        _basketHub = basketHub;
    }

    public async Task HandleEventAsync(OrderCreatedEto eventData)
    {
        await _basketManager.RemoveItemsAsync(
            eventData.UserId,
            eventData.ProductsWithAmounts
        );
    }

    public async Task HandleEventAsync(BasketUpdatedEto eventData)
    {
        await _basketHub
            .Clients
            .User(eventData.UserId.ToString())
            .SendAsync(
                "BasketUpdated",
                new
                {
                    eventData.ItemCountInBasket                    
                }
            );
    }
}