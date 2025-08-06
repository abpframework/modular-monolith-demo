using Shopularity.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Shopularity;

public abstract class ShopularityComponentBase : AbpComponentBase
{
    protected ShopularityComponentBase()
    {
        LocalizationResource = typeof(ShopularityResource);
    }
}
