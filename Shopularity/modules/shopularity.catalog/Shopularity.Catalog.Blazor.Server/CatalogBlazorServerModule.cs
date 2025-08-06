using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace Shopularity.Catalog.Blazor.Server;

[DependsOn(
    typeof(AbpAspNetCoreComponentsServerThemingModule),
    typeof(CatalogBlazorModule)
    )]
public class CatalogBlazorServerModule : AbpModule
{

}
