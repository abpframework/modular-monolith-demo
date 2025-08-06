using Shopularity.Basket.Localization;
using Volo.Abp.Application.Services;

namespace Shopularity.Basket;

public abstract class BasketAppService : ApplicationService
{
    protected BasketAppService()
    {
        LocalizationResource = typeof(BasketResource);
        ObjectMapperContext = typeof(BasketModule);
    }
}
