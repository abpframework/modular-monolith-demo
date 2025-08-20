using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shopularity.Catalog.Products.Admin;

namespace Shopularity.Catalog.Products;

public class ProductsIntegrationService : CatalogAppService, IProductsIntegrationService
{
    private readonly IProductRepository _productRepository;

    public ProductsIntegrationService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<List<ProductDto>> GetProductsAsync(List<Guid> ids)
    {
        var products = await _productRepository.GetListAsync(ids);
        
        return ObjectMapper.Map<List<Product>, List<ProductDto>>(products);
    }
}