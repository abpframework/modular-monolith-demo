using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Shopularity.Basket.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace Shopularity.Basket.SignalR.Events;

public class BasketSignalREventHandler
    : IDistributedEventHandler<BasketUpdatedEto>,
      ITransientDependency
{
    private readonly IHubContext<BasketHub> _basketHub;

    public BasketSignalREventHandler(IHubContext<BasketHub> basketHub)
    {
        _basketHub = basketHub;
    }
    
    public async Task HandleEventAsync(BasketUpdatedEto eventData)
    {
        await _basketHub
            .Clients
            .User(eventData.UserId.ToString())
            .SendAsync(
                "BasketUpdated",
                eventData
            );
    }
}