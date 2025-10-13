using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using MiniExcelLibs;
using Shopularity.Catalog.Domain.Categories;
using Shopularity.Catalog.Domain.Products;
using Shopularity.Catalog.Permissions;
using Shopularity.Catalog.Shared;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.BlobStoring;
using Volo.Abp.Caching;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;

namespace Shopularity.Catalog.Services.Products.Admin;

[RemoteService(IsEnabled = false)]
[Authorize(CatalogPermissions.Products.Default)]
public class ProductsAdminAppService : CatalogAppService, IProductsAdminAppService
{
    private readonly IDistributedCache<ProductDownloadTokenCacheItem, string> _downloadTokenCache;
    private readonly IBlobContainer _blobContainer;
    private readonly IProductRepository _productRepository;
    private readonly ProductManager _productManager;
    private readonly ICategoryRepository _categoryRepository;

    public ProductsAdminAppService(
        IProductRepository productRepository,
        ProductManager productManager,
        IDistributedCache<ProductDownloadTokenCacheItem, string> downloadTokenCache,
        IBlobContainer blobContainer,
        ICategoryRepository categoryRepository)
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
        var result = new PagedResultDto<ProductWithNavigationPropertiesDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<ProductWithNavigationProperties>, List<ProductWithNavigationPropertiesDto>>(items)
        };

        foreach (var product in result.Items)
        {
            var imageStream = await _blobContainer.GetOrNullAsync(product.Product.Id.ToString());
            
            product.Product.Image = imageStream != null ? await imageStream.GetAllBytesAsync() : null;
        }
        
        return result;
    }

    public virtual async Task<ProductWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
    {
        var productWithNavigationProperties = ObjectMapper.Map<ProductWithNavigationProperties, ProductWithNavigationPropertiesDto>
            (await _productRepository.GetWithNavigationPropertiesAsync(id));
            
        var imageStream = await _blobContainer.GetOrNullAsync(id.ToString());
            
        productWithNavigationProperties.Product.Image = imageStream != null ? await imageStream.GetAllBytesAsync() : null;

        return productWithNavigationProperties;
    }

    public virtual async Task<ProductDto> GetAsync(Guid id)
    {
        var product = ObjectMapper.Map<Product, ProductDto>(await _productRepository.GetAsync(id));

        var imageStream = await _blobContainer.GetOrNullAsync(id.ToString());
            
        product.Image = imageStream != null ? await imageStream.GetAllBytesAsync() : null;
            
        return product;
    }
        
    public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetCategoryLookupAsync(LookupRequestDto input)
    {
        var query = (await _categoryRepository.GetQueryableAsync())
            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                x => x.Name != null &&
                     x.Name.Contains(input.Filter));

        var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Category>();
        var totalCount = query.Count();
        
        return new PagedResultDto<LookupDto<Guid>>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<Category>, List<LookupDto<Guid>>>(lookupData)
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
        
    public virtual async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");

        await _downloadTokenCache.SetAsync(
            token,
            new ProductDownloadTokenCacheItem { Token = token },
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

        return new DownloadTokenResultDto
        {
            Token = token
        };
    }
}