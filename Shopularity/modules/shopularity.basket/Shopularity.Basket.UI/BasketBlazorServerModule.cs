using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;
using Shopularity.Basket.Blazor;

namespace Shopularity.Basket.UI;

[DependsOn(
    typeof(AbpAspNetCoreComponentsServerThemingModule),
    typeof(BasketBlazorModule)
    )]
public class BasketBlazorServerModule : AbpModule
{

}
