using Shopularity.Basket.Localization;
using Volo.Abp.Application.Services;

namespace Shopularity.Basket.Services;

public abstract class BasketAppServiceBase : ApplicationService
{
    protected BasketAppServiceBase()
    {
        LocalizationResource = typeof(BasketResource);
        ObjectMapperContext = typeof(BasketModule);
    }
}
