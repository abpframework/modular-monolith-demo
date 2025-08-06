using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;
using Shopularity.Payment.Blazor;

namespace Shopularity.Payment.UI;

[DependsOn(
    typeof(AbpAspNetCoreComponentsServerThemingModule),
    typeof(PaymentBlazorModule)
    )]
public class PaymentBlazorServerModule : AbpModule
{

}
