using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shopularity.Catalog.Products.Admin;
using Shopularity.Catalog.Products.Public;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Shopularity.Catalog.Products;

[IntegrationService]
public interface IProductsIntegrationService: IApplicationService
{
    Task<List<ProductDto>> GetProductsAsync(List<ProductIdsWithAmountDto> productIdsWithAmount);

    Task<List<ProductPublicDto>> GetProductsAsync(List<Guid> ids);
    
    Task<bool> CheckStockAsync(Guid id, int amount);
}