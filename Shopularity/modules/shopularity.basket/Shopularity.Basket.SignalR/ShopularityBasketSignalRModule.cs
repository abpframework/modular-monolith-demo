using Shopularity.Basket;
using Volo.Abp.Modularity;

namespace Shopularity.Basket.SignalR;

[DependsOn(
    typeof(BasketContractsModule)
)]
public class ShopularityBasketSignalRModule : AbpModule
{

}