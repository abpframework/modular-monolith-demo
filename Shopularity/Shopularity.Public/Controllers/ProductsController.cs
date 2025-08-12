using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Catalog.Products;
using Shopularity.Catalog.Products.Public;
using Shopularity.Public.Components.ProductList;
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
    
    [HttpGet("render")]
    public async Task<ViewComponentResult> Render([FromQuery] int skip = 0, [FromQuery] int take = 12, [FromQuery] string sorting = "Product.Name")
    {
        var result = await ProductsPublicAppService.GetListAsync(new GetProductsInput() {
            SkipCount = skip,
            MaxResultCount = take,
            Sorting = sorting
        });

        return ViewComponent("ProductList", new ProductListViewModel {
            TotalCount = result.TotalCount,
            Items = result.Items
        });
    }
}