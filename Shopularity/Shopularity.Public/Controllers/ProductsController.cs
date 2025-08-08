using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Catalog.Products;
using Shopularity.Catalog.Products.Public;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Public.Controllers;

[RemoteService(Name = "Catalog")]
[Area("catalog")]
[ControllerName("ProductPublic")]
[Route("api/catalog/public/products")]
public class ProductsController : AbpController, IProductsPublicAppService
{
    protected IProductsPublicAppService ProductsPublicAppService;

    public ProductsController(IProductsPublicAppService productsPublicAppService)
    {
        ProductsPublicAppService = productsPublicAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<ProductWithNavigationPropertiesPublicDto>> GetListAsync(GetProductsInput input)
    {
        return ProductsPublicAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<ProductWithNavigationPropertiesPublicDto> GetAsync(Guid id)
    {
        return ProductsPublicAppService.GetAsync(id);
    }
}