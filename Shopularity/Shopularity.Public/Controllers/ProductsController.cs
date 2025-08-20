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
    public virtual Task<PagedResultDto<ProductWithNavigationPropertiesPublicDto>> GetListAsync(GetProductsPublicInput input)
    {
        return ProductsPublicAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("ids")]
    public Task<ListResultDto<ProductWithNavigationPropertiesPublicDto>> GetListByIdsAsync(GetListByIdsInput input)
    {
        return ProductsPublicAppService.GetListByIdsAsync(input);
    }

    [HttpPost]
    [Route("request")]
    public Task RequestProductsAsync(ProductsRequestedInput input)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<ProductWithNavigationPropertiesPublicDto> GetAsync(Guid id)
    {
        return ProductsPublicAppService.GetAsync(id);
    }
    
    [HttpGet]
    [Route("render")]
    public async Task<ViewComponentResult> Render(GetProductsPublicInput input)
    {
        var result = await ProductsPublicAppService.GetListAsync(input);

        return ViewComponent("ProductList", new ProductListViewModel {
            TotalCount = result.TotalCount,
            Items = result.Items
        });
    }
}