using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;
using Shopularity.Ordering.Blazor;

namespace Shopularity.Ordering.UI;

[DependsOn(
    typeof(AbpAspNetCoreComponentsServerThemingModule),
    typeof(OrderingBlazorModule)
    )]
public class OrderingBlazorServerModule : AbpModule
{

}
