using Shopularity.Ordering.Localization;
using Volo.Abp.Application.Services;

namespace Shopularity.Ordering.Services;

public abstract class OrderingAppService : ApplicationService
{
    protected OrderingAppService()
    {
        LocalizationResource = typeof(OrderingResource);
        ObjectMapperContext = typeof(OrderingModule);
    }
}
