using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Shopularity.Basket.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace Shopularity.Basket.SignalR.Events;

public class BasketEventHandler
    : IDistributedEventHandler<BasketChangedEto>,
        ITransientDependency
{
    private readonly IHubContext<BasketHub> _basketHub;

    public BasketEventHandler(IHubContext<BasketHub> basketHub)
    {
        _basketHub = basketHub;
    }
    
    public async Task HandleEventAsync(BasketChangedEto eventData)
    {
        await _basketHub
            .Clients
            .User(eventData.UserId.ToString())
            .SendAsync(
                "BasketChange",
                eventData.Items
            );
    }
}