using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Shopularity.Public.Bundling;

public class ShopularityPublicGlobalStyleContributor : BundleContributor
{
    public async override Task ConfigureBundleAsync(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/Pages/Global.css");
    }
}