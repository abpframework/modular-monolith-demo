using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Shopularity.Basket.Blazor.WebAssembly;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule),
    typeof(BasketBlazorModule)
    )]
public class BasketBlazorWebAssemblyModule : AbpModule
{

}
