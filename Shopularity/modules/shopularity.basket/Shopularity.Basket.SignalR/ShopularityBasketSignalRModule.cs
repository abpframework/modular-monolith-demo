using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Modularity;

namespace Shopularity.Basket.SignalR;

[DependsOn(
    typeof(BasketContractsModule),
    typeof(AbpAspNetCoreSignalRModule)
)]
public class ShopularityBasketSignalRModule : AbpModule
{

}