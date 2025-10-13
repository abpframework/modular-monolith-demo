using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shopularity.Catalog.Services.Products.Admin;
using Shopularity.Catalog.Services.Products.Public;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Shopularity.Catalog.Services.Products.Integration;

[IntegrationService]
public interface IProductsIntegrationService: IApplicationService
{
    Task<List<ProductDto>> GetProductsAsync(List<Guid> ids);

    Task<List<ProductPublicDto>> GetPublicProductsAsync(List<Guid> ids);
    
    Task<bool> CheckStockAsync(Guid id, int amount);
}