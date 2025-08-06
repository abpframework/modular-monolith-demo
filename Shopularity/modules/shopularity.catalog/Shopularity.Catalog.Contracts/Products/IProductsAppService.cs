using Shopularity.Catalog.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Shopularity.Catalog.Products
{
    public interface IProductsAppService : IApplicationService
    {

        Task<PagedResultDto<ProductWithNavigationPropertiesDto>> GetListAsync(GetProductsInput input);

        Task<ProductWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<ProductDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetCategoryLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<ProductDto> CreateAsync(ProductCreateDto input);

        Task<ProductDto> UpdateAsync(Guid id, ProductUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(ProductExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> productIds);

        Task DeleteAllAsync(GetProductsInput input);
        Task<Shopularity.Catalog.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}