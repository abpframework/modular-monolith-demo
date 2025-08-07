using Shopularity.Catalog.Shared;
using Asp.Versioning;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace Shopularity.Catalog.Products;

[RemoteService(Name = "Catalog")]
[Area("catalog")]
[ControllerName("Product")]
[Route("api/catalog/products")]
public class ProductController : AbpController, IProductsAppService
{
    protected IProductsAppService _productsAppService;

    public ProductController(IProductsAppService productsAppService)
    {
        _productsAppService = productsAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<ProductWithNavigationPropertiesDto>> GetListAsync(GetProductsInput input)
    {
        return _productsAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("with-navigation-properties/{id}")]
    public virtual Task<ProductWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
    {
        return _productsAppService.GetWithNavigationPropertiesAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<ProductDto> GetAsync(Guid id)
    {
        return _productsAppService.GetAsync(id);
    }

    [HttpGet]
    [Route("category-lookup")]
    public virtual Task<PagedResultDto<LookupDto<Guid>>> GetCategoryLookupAsync(LookupRequestDto input)
    {
        return _productsAppService.GetCategoryLookupAsync(input);
    }

    [HttpPost]
    public virtual Task<ProductDto> CreateAsync(ProductCreateDto input)
    {
        return _productsAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<ProductDto> UpdateAsync(Guid id, ProductUpdateDto input)
    {
        return _productsAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _productsAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("as-excel-file")]
    public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(ProductExcelDownloadDto input)
    {
        return _productsAppService.GetListAsExcelFileAsync(input);
    }

    [HttpGet]
    [Route("download-token")]
    public virtual Task<Shopularity.Catalog.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        return _productsAppService.GetDownloadTokenAsync();
    }

    [HttpDelete]
    [Route("")]
    public virtual Task DeleteByIdsAsync(List<Guid> productIds)
    {
        return _productsAppService.DeleteByIdsAsync(productIds);
    }

    [HttpDelete]
    [Route("all")]
    public virtual Task DeleteAllAsync(GetProductsInput input)
    {
        return _productsAppService.DeleteAllAsync(input);
    }
}