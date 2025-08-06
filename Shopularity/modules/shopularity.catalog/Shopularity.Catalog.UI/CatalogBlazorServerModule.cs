using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;
using Shopularity.Catalog.Blazor;

namespace Shopularity.Catalog.UI;

[DependsOn(
    typeof(AbpAspNetCoreComponentsServerThemingModule),
    typeof(CatalogBlazorModule)
    )]
public class CatalogBlazorServerModule : AbpModule
{

}
