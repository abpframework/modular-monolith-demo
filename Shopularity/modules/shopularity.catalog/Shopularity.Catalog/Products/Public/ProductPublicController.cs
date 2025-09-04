using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Catalog.Products.Public;

[RemoteService(Name = "Catalog")]
[Area("catalog")]
[ControllerName("ProductPublic")]
[Route("api/catalog/public/products")]
public class ProductPublicController : AbpController, IProductsPublicAppService
{
    protected IProductsPublicAppService ProductsPublicAppService; //TODO: private

    public ProductPublicController(IProductsPublicAppService productsPublicAppService)
    {
        ProductsPublicAppService = productsPublicAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<ProductWithNavigationPropertiesPublicDto>> GetListAsync(GetProductsPublicInput input)
    {
        return ProductsPublicAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<ProductWithNavigationPropertiesPublicDto> GetAsync(Guid id)
    {
        return ProductsPublicAppService.GetAsync(id);
    }

    [HttpGet]
    [Route("image-as-bytes/{id}")]
    public Task<byte[]> GetImageAsByteArrayAsync(Guid id)
    {
        return ProductsPublicAppService.GetImageAsByteArrayAsync(id);
    }
}