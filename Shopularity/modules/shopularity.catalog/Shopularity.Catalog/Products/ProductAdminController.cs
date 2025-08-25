using Shopularity.Catalog.Shared;
using Asp.Versioning;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Catalog.Products.Admin;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace Shopularity.Catalog.Products;

[RemoteService(Name = "Catalog")]
[Area("catalog")]
[ControllerName("Product")]
[Route("api/catalog/products")]
public class ProductAdminController : AbpController, IProductsAdminAppService
{
    protected IProductsAdminAppService ProductsAdminAppService;

    public ProductAdminController(IProductsAdminAppService productsAdminAppService)
    {
        ProductsAdminAppService = productsAdminAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<ProductWithNavigationPropertiesDto>> GetListAsync(GetProductsInput input)
    {
        return ProductsAdminAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("with-navigation-properties/{id}")]
    public virtual Task<ProductWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
    {
        return ProductsAdminAppService.GetWithNavigationPropertiesAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<ProductDto> GetAsync(Guid id)
    {
        return ProductsAdminAppService.GetAsync(id);
    }

    [HttpGet]
    [Route("category-lookup")]
    public virtual Task<PagedResultDto<LookupDto<Guid>>> GetCategoryLookupAsync(LookupRequestDto input)
    {
        return ProductsAdminAppService.GetCategoryLookupAsync(input);
    }

    [HttpPost]
    public virtual Task<ProductDto> CreateAsync(ProductCreateDto input)
    {
        return ProductsAdminAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<ProductDto> UpdateAsync(Guid id, ProductUpdateDto input)
    {
        return ProductsAdminAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return ProductsAdminAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("as-excel-file")]
    public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(ProductExcelDownloadDto input)
    {
        return ProductsAdminAppService.GetListAsExcelFileAsync(input);
    }

    [HttpGet]
    [Route("download-token")]
    public virtual Task<DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        return ProductsAdminAppService.GetDownloadTokenAsync();
    }

    [HttpDelete]
    [Route("")]
    public virtual Task DeleteByIdsAsync(List<Guid> productIds)
    {
        return ProductsAdminAppService.DeleteByIdsAsync(productIds);
    }

    [HttpDelete]
    [Route("all")]
    public virtual Task DeleteAllAsync(GetProductsInput input)
    {
        return ProductsAdminAppService.DeleteAllAsync(input);
    }
}