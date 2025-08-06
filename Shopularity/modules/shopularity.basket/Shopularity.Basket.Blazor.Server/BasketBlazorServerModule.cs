using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace Shopularity.Basket.Blazor.Server;

[DependsOn(
    typeof(AbpAspNetCoreComponentsServerThemingModule),
    typeof(BasketBlazorModule)
    )]
public class BasketBlazorServerModule : AbpModule
{

}
