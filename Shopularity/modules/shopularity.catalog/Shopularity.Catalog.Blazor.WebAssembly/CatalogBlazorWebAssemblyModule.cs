using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Shopularity.Catalog.Blazor.WebAssembly;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule),
    typeof(CatalogBlazorModule)
    )]
public class CatalogBlazorWebAssemblyModule : AbpModule
{

}
