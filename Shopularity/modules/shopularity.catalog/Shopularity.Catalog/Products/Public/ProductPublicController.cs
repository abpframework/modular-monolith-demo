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
    private readonly IProductsPublicAppService _productsPublicAppService; //TODO: private

    public ProductPublicController(IProductsPublicAppService productsPublicAppService)
    {
        _productsPublicAppService = productsPublicAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<ProductWithNavigationPropertiesPublicDto>> GetListAsync(GetProductsPublicInput input)
    {
        return _productsPublicAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<ProductWithNavigationPropertiesPublicDto> GetAsync(Guid id)
    {
        return _productsPublicAppService.GetAsync(id);
    }

    [HttpGet]
    [Route("image-as-bytes/{id}")]
    public Task<byte[]> GetImageAsByteArrayAsync(Guid id)
    {
        return _productsPublicAppService.GetImageAsByteArrayAsync(id);
    }
}