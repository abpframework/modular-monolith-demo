using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Shopularity.Catalog.Products.Public;

public interface IProductsPublicAppService : IApplicationService
{
    Task<PagedResultDto<ProductWithNavigationPropertiesPublicDto>> GetListAsync(GetProductsPublicInput input);

    Task<ListResultDto<ProductWithNavigationPropertiesPublicDto>> GetListByIdsAsync(GetListByIdsInput input);

    Task<ProductWithNavigationPropertiesPublicDto> GetAsync(Guid id);
}