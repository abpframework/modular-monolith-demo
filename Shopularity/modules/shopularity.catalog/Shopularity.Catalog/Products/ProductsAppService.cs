using Shopularity.Catalog.Shared;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Shopularity.Catalog.Permissions;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.BlobStoring;

namespace Shopularity.Catalog.Products;

[RemoteService(IsEnabled = false)]
[Authorize(CatalogPermissions.Products.Default)]
public class ProductsAppService : CatalogAppService, IProductsAppService
{
    protected IDistributedCache<ProductDownloadTokenCacheItem, string> _downloadTokenCache;
    private readonly IBlobContainer _blobContainer;
    protected IProductRepository _productRepository;
    protected ProductManager _productManager;

    protected IRepository<Categories.Category, Guid> _categoryRepository;

    public ProductsAppService(
        IProductRepository productRepository,
        ProductManager productManager,
        IDistributedCache<ProductDownloadTokenCacheItem, string> downloadTokenCache,
        IBlobContainer blobContainer,
        IRepository<Categories.Category, Guid> categoryRepository)
    {
        _downloadTokenCache = downloadTokenCache;
        _blobContainer = blobContainer;
        _productRepository = productRepository;
        _productManager = productManager; _categoryRepository = categoryRepository;

    }

    public virtual async Task<PagedResultDto<ProductWithNavigationPropertiesDto>> GetListAsync(GetProductsInput input)
    {
        var totalCount = await _productRepository.GetCountAsync(input.FilterText, input.Name, input.Description, input.PriceMin, input.PriceMax, input.StockCountMin, input.StockCountMax, input.CategoryId);
        var items = await _productRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Name, input.Description, input.PriceMin, input.PriceMax, input.StockCountMin, input.StockCountMax, input.CategoryId, input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<ProductWithNavigationPropertiesDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<ProductWithNavigationProperties>, List<ProductWithNavigationPropertiesDto>>(items)
        };
    }

    public virtual async Task<ProductWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
    {
        var productWithNavigationProperties = ObjectMapper.Map<ProductWithNavigationProperties, ProductWithNavigationPropertiesDto>
            (await _productRepository.GetWithNavigationPropertiesAsync(id));
            
        var imageStream = await _blobContainer.GetOrNullAsync(id.ToString());
            
        productWithNavigationProperties.Product.Image = imageStream != null ? ReadAllBytesFromStream(imageStream) : null;

        return productWithNavigationProperties;
    }

    public virtual async Task<ProductDto> GetAsync(Guid id)
    {
        var product = ObjectMapper.Map<Product, ProductDto>(await _productRepository.GetAsync(id));

        var imageStream = await _blobContainer.GetOrNullAsync(id.ToString());
            
        product.Image = imageStream != null ? ReadAllBytesFromStream(imageStream) : null;
            
        return product;
    }
        
    public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetCategoryLookupAsync(LookupRequestDto input)
    {
        var query = (await _categoryRepository.GetQueryableAsync())
            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                x => x.Name != null &&
                     x.Name.Contains(input.Filter));

        var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Shopularity.Catalog.Categories.Category>();
        var totalCount = query.Count();
        return new PagedResultDto<LookupDto<Guid>>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<Categories.Category>, List<LookupDto<Guid>>>(lookupData)
        };
    }

    [Authorize(CatalogPermissions.Products.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _productRepository.DeleteAsync(id);
    }

    [Authorize(CatalogPermissions.Products.Create)]
    public virtual async Task<ProductDto> CreateAsync(ProductCreateDto input)
    {
        var product = await _productManager.CreateAsync(
            input.CategoryId,
            input.Name,
            input.Price,
            input.StockCount,
            input.Image,
            input.Description
        );

        return ObjectMapper.Map<Product, ProductDto>(product);
    }

    [Authorize(CatalogPermissions.Products.Edit)]
    public virtual async Task<ProductDto> UpdateAsync(Guid id, ProductUpdateDto input)
    {

        var product = await _productManager.UpdateAsync(
            id,
            input.CategoryId,
            input.Name,
            input.Price,
            input.StockCount,
            input.Image,
            input.Description,
            input.ConcurrencyStamp
        );

        return ObjectMapper.Map<Product, ProductDto>(product);
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ProductExcelDownloadDto input)
    {
        var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var products = await _productRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Name, input.Description, input.PriceMin, input.PriceMax, input.StockCountMin, input.StockCountMax, input.CategoryId);
        var items = products.Select(item => new
        {
            Name = item.Product.Name,
            Description = item.Product.Description,
            Price = item.Product.Price,
            StockCount = item.Product.StockCount,

            Category = item.Category?.Name,

        });

        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(items);
        memoryStream.Seek(0, SeekOrigin.Begin);

        return new RemoteStreamContent(memoryStream, "Products.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [Authorize(CatalogPermissions.Products.Delete)]
    public virtual async Task DeleteByIdsAsync(List<Guid> productIds)
    {
        await _productRepository.DeleteManyAsync(productIds);
    }

    [Authorize(CatalogPermissions.Products.Delete)]
    public virtual async Task DeleteAllAsync(GetProductsInput input)
    {
        await _productRepository.DeleteAllAsync(input.FilterText, input.Name, input.Description, input.PriceMin, input.PriceMax, input.StockCountMin, input.StockCountMax, input.CategoryId);
    }
        
    public virtual async Task<Shopularity.Catalog.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");

        await _downloadTokenCache.SetAsync(
            token,
            new ProductDownloadTokenCacheItem { Token = token },
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

        return new Shopularity.Catalog.Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }

    private byte[] ReadAllBytesFromStream(Stream input)
    {
        using var memoryStream = new MemoryStream();
        input.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}