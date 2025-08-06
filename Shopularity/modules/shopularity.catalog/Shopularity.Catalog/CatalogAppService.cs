using Shopularity.Catalog.Localization;
using Volo.Abp.Application.Services;

namespace Shopularity.Catalog;

public abstract class CatalogAppService : ApplicationService
{
    protected CatalogAppService()
    {
        LocalizationResource = typeof(CatalogResource);
        ObjectMapperContext = typeof(CatalogModule);
    }
}
