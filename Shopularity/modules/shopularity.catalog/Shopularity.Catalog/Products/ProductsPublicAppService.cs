using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Shopularity.Catalog.Categories;
using Shopularity.Catalog.Products.Admin;
using Shopularity.Catalog.Products.Events;
using Shopularity.Catalog.Products.Public;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;

namespace Shopularity.Catalog.Products;

[RemoteService(IsEnabled = false)]
public class ProductsPublicAppService : CatalogAppService, IProductsPublicAppService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBlobContainer _blobContainer;
    private readonly IDistributedEventBus _eventBus;

    public ProductsPublicAppService(IProductRepository productRepository, ICategoryRepository categoryRepository, IBlobContainer blobContainer, IDistributedEventBus eventBus)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _blobContainer = blobContainer;
        _eventBus = eventBus;
    }

    public virtual async Task<PagedResultDto<ProductWithNavigationPropertiesPublicDto>> GetListAsync(GetProductsPublicInput input)
    {
        Guid? categoryId = null;

        if (!input.CategoryName.IsNullOrWhiteSpace())
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(x =>
                x.Name.Equals(input.CategoryName));
            categoryId = category?.Id;
        }
        
        var totalCount = await _productRepository.GetCountAsync(categoryId: categoryId);
        var items = await _productRepository.GetListWithNavigationPropertiesAsync(categoryId: categoryId, sorting: input.Sorting, maxResultCount: input.MaxResultCount, skipCount: input.SkipCount);
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

    public virtual async Task<ListResultDto<ProductWithNavigationPropertiesPublicDto>> GetListByIdsAsync(GetListByIdsInput input)
    {
        var items = await _productRepository.GetListWithNavigationPropertiesAsync(input.Ids);
        var result = new ListResultDto<ProductWithNavigationPropertiesPublicDto>
        {
            Items = ObjectMapper.Map<List<ProductWithNavigationProperties>, List<ProductWithNavigationPropertiesPublicDto>>(items)
        };

        foreach (var product in result.Items)
        {
            var imageStream = await _blobContainer.GetOrNullAsync(product.Product.Id.ToString());
            
            product.Product.Image = imageStream != null ? ReadAllBytesFromStream(imageStream) : null;
        }
        
        return result;
    }    
    
    public async Task RequestProductsAsync(ProductsRequestedInput input)
    {
        var products = await _productRepository.GetListAsync(input.Products.Select(x=> x.Key).ToList());

        foreach (var product in products)
        {
            var amount = input.Products[product.Id];

            if (amount > product.StockCount)
            {
                //todo: business exception
                throw new UserFriendlyException($"Not Enough Stock!! {amount} > {product.StockCount}");
            }

            product.StockCount -= amount;

            await _productRepository.UpdateAsync(product);
        }
        
        var productsAsDto = ObjectMapper.Map<List<Product>, List<ProductDto>>(products);

        await _eventBus.PublishAsync(new ProductsRequestCompletedEto
        {
            Products = productsAsDto.Select(x=>
                new KeyValuePair<ProductDto, int>(x, input.Products[x.Id])
            ).ToList(),
            RequesterId = input.RequesterId
        });
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