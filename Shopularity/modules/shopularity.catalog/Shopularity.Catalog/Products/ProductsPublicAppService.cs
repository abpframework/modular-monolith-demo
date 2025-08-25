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

    public ProductsPublicAppService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IBlobContainer blobContainer)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _blobContainer = blobContainer;
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
        
        return result;
    }

    public virtual async Task<ListResultDto<ProductWithNavigationPropertiesPublicDto>> GetListByIdsAsync(GetListByIdsInput input)
    {
        var items = await _productRepository.GetListWithNavigationPropertiesAsync(input.Ids);
        var result = new ListResultDto<ProductWithNavigationPropertiesPublicDto>
        {
            Items = ObjectMapper.Map<List<ProductWithNavigationProperties>, List<ProductWithNavigationPropertiesPublicDto>>(items)
        };
        
        return result;
    }    

    public virtual async Task<ProductWithNavigationPropertiesPublicDto> GetAsync(Guid id)
    {
        var productWithNavigationProperties = ObjectMapper.Map<ProductWithNavigationProperties, ProductWithNavigationPropertiesPublicDto>
            (await _productRepository.GetWithNavigationPropertiesAsync(id));
            
        return productWithNavigationProperties;
    }

    public async Task<byte[]> GetImageAsByteArrayAsync(Guid id)
    {
        var imageStream = await _blobContainer.GetOrNullAsync(id.ToString());

        if (imageStream == null)
        {
            return [];
        }

        return ReadAllBytesFromStream(imageStream);
    }

    private byte[] ReadAllBytesFromStream(Stream input)
    {
        using var memoryStream = new MemoryStream();
        input.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}