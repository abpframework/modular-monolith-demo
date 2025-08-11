using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.SignalR;
using Volo.Abp.Modularity;

namespace Volo.Chat.Web.Bundling;

[DependsOn(
    typeof(SignalRBrowserScriptContributor)
)]
public class ShopularityPublicGlobalScriptContributor : BundleContributor
{
    public async override Task ConfigureBundleAsync(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/Pages/Basket/basket.js");
    }
}
