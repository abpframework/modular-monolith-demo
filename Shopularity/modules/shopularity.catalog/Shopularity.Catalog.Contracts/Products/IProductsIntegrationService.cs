using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shopularity.Catalog.Products.Admin;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Shopularity.Catalog.Products;

[IntegrationService]
public interface IProductsIntegrationService: IApplicationService
{
    Task<List<ProductDto>> GetProductsWithAmountControlAsync(List<ProductIdsWithAmountDto> productIdsWithAmount);

    Task<bool> CheckStockAsync(Guid id, int amount);
}