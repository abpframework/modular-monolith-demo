using Volo.Abp.Application.Services;
using Shopularity.Localization;

namespace Shopularity.Services;

/* Inherit your application services from this class. */
public abstract class ShopularityAppServiceBase : ApplicationService
{
    protected ShopularityAppServiceBase()
    {
        LocalizationResource = typeof(ShopularityResource);
    }
}