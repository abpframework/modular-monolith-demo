using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Catalog.Services.Products.Public;
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

    [HttpGet]
    [Route("image/{id}")]
    public async Task<IActionResult> GetImageAsFileAsync(Guid id)
    {
        var imageBytes = await ProductsPublicAppService.GetImageAsByteArrayAsync(id);

        if (imageBytes.Length == 0)
        {
            return NotFound();
        }

        return File(imageBytes, "image/png");
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