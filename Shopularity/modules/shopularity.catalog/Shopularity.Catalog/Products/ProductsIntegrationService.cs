using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopularity.Catalog.Products.Admin;
using Shopularity.Catalog.Products.Public;
using Volo.Abp;

namespace Shopularity.Catalog.Products;

public class ProductsIntegrationService : CatalogAppService, IProductsIntegrationService
{
    private readonly IProductRepository _productRepository;

    public ProductsIntegrationService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<List<ProductDto>> GetProductsWithAmountControlAsync(List<ProductIdsWithAmountDto> productIdsWithAmount)
    {
        var products = await _productRepository.GetListAsync(productIdsWithAmount.Select(x=> x.ProductId).ToList());

        foreach (var product in products)
        {
            var amount = productIdsWithAmount.First(x => x.ProductId == product.Id).Amount;

            if (product.StockCount < amount)
            {
                throw new BusinessException(CatalogErrorCodes.NotEnoughStock);
            }
        }
        
        return ObjectMapper.Map<List<Product>, List<ProductDto>>(products);
    }
    
    public async Task<List<ProductPublicDto>> GetProductsAsync(List<Guid> ids)
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