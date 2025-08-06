using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Shopularity;

public class ShopularityStyleBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add(new BundleFile("main.css", true));
    }
}
