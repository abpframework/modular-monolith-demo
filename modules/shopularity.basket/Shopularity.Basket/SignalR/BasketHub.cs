using Microsoft.AspNetCore.Authorization;
using Volo.Abp.AspNetCore.SignalR;

namespace Shopularity.Basket.SignalR;

[Authorize]
public class BasketHub : AbpHub
{
    
}