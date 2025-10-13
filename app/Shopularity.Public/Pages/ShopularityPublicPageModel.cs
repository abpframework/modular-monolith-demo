using Shopularity.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Shopularity.Public.Pages;

public abstract class ShopularityPublicPageModel : AbpPageModel
{
    protected ShopularityPublicPageModel()
    {
        LocalizationResourceType = typeof(ShopularityResource);
    }
}
