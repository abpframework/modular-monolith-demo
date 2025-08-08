using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Shopularity.Catalog.Products.Public;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BlobStoring;

namespace Shopularity.Catalog.Products;

[RemoteService(IsEnabled = false)]
public class ProductsPublicAppService : CatalogAppService, IProductsPublicAppService
{
    private readonly IProductRepository _productRepository;
    private readonly IBlobContainer _blobContainer;

    public ProductsPublicAppService(IProductRepository productRepository, IBlobContainer blobContainer)
    {
        _productRepository = productRepository;
        _blobContainer = blobContainer;
    }

    public virtual async Task<PagedResultDto<ProductWithNavigationPropertiesPublicDto>> GetListAsync(GetProductsInput input)
    {
        var totalCount = await _productRepository.GetCountAsync(input.FilterText, input.Name, input.Description, input.PriceMin, input.PriceMax, input.StockCountMin, input.StockCountMax, input.CategoryId);
        var items = await _productRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Name, input.Description, input.PriceMin, input.PriceMax, input.StockCountMin, input.StockCountMax, input.CategoryId, input.Sorting, input.MaxResultCount, input.SkipCount);
        var result = new PagedResultDto<ProductWithNavigationPropertiesPublicDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<ProductWithNavigationProperties>, List<ProductWithNavigationPropertiesPublicDto>>(items)
        };

        foreach (var product in result.Items)
        {
            var imageStream = await _blobContainer.GetOrNullAsync(product.Product.Id.ToString());
            
            product.Product.Image = imageStream != null ? ReadAllBytesFromStream(imageStream) : null;
        }
        
        return result;
    }

    public virtual async Task<ProductWithNavigationPropertiesPublicDto> GetAsync(Guid id)
    {
        var productWithNavigationProperties = ObjectMapper.Map<ProductWithNavigationProperties, ProductWithNavigationPropertiesPublicDto>
            (await _productRepository.GetWithNavigationPropertiesAsync(id));
            
        var imageStream = await _blobContainer.GetOrNullAsync(id.ToString());
            
        productWithNavigationProperties.Product.Image = imageStream != null ? ReadAllBytesFromStream(imageStream) : null;

        return productWithNavigationProperties;
    }

    private byte[] ReadAllBytesFromStream(Stream input)
    {
        using var memoryStream = new MemoryStream();
        input.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}