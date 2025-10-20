using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shopularity.Catalog.Shared;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Shopularity.Catalog.Services.Products.Admin;

public interface IProductsAdminAppService : IApplicationService
{

    Task<PagedResultDto<ProductWithNavigationPropertiesDto>> GetListAsync(GetProductsInput input);

    Task<ProductWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

    Task<PagedResultDto<LookupDto<Guid>>> GetCategoryLookupAsync(LookupRequestDto input);

    Task DeleteAsync(Guid id);

    Task<ProductDto> CreateAsync(ProductCreateDto input);

    Task<ProductDto> UpdateAsync(Guid id, ProductUpdateDto input);

    Task<IRemoteStreamContent> GetListAsExcelFileAsync(ProductExcelDownloadDto input);
    
    Task DeleteByIdsAsync(List<Guid> productIds);

    Task DeleteAllAsync(GetProductsInput input);
    
    Task<DownloadTokenResultDto> GetDownloadTokenAsync();

}