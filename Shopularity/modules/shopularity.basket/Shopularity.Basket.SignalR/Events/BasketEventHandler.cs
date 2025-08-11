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
    private readonly BasketHub _basketHub;
    private readonly ICurrentUser _currentUser;

    public BasketEventHandler(BasketHub basketHub, ICurrentUser currentUser)
    {
        _basketHub = basketHub;
        _currentUser = currentUser;
    }
    
    public async Task HandleEventAsync(BasketChangedEto eventData)
    {
        await _basketHub
            .Clients
            .User(_currentUser.GetId().ToString())
            .SendAsync(
                "BasketChange",
                eventData
            );
    }
}