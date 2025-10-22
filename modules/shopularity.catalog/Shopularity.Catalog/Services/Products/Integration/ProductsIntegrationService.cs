using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shopularity.Catalog.Domain.Products;
using Shopularity.Catalog.Services.Products.Admin;
using Shopularity.Catalog.Services.Products.Public;

namespace Shopularity.Catalog.Services.Products.Integration;

public class ProductsIntegrationService : CatalogAppService, IProductsIntegrationService
{
    private readonly IProductRepository _productRepository;

    public ProductsIntegrationService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<List<ProductPublicDto>> GetPublicProductsAsync(List<Guid> ids)
    {
        var products = await _productRepository.GetListAsync(ids);

        return ObjectMapper.Map<List<Product>, List<ProductPublicDto>>(products);
    }

    public async Task<bool> CheckStockAsync(Guid id, int amount)
    {
        var product = await _productRepository.GetAsync(id);

        return product.StockCount >= amount;
    }
}