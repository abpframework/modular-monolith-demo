using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Shopularity.Catalog.Products.Public;

public interface IProductsPublicAppService : IApplicationService
{
    Task<PagedResultDto<ProductWithNavigationPropertiesPublicDto>> GetListAsync(GetProductsInput input);

    Task<ProductWithNavigationPropertiesPublicDto> GetAsync(Guid id);
}