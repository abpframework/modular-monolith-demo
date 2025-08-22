using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Basket.Services;
using Shopularity.Catalog.Products;
using Shopularity.Catalog.Products.Public;
using Shopularity.Public.Components.Basket;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Public.Controllers;

[RemoteService(Name = "Catalog")]
[Area("catalog")]
[ControllerName("Basket")]
[Route("api/basket/basket")]
public class BasketController : AbpController, IBasketAppService
{
    protected IBasketAppService BasketAppService;
    protected IProductsPublicAppService ProductsPublicAppService;

    public BasketController(IBasketAppService basketAppService, IProductsPublicAppService productsPublicAppService)
    {
        BasketAppService = basketAppService;
        ProductsPublicAppService = productsPublicAppService;
    }

    [HttpGet]
    [Route("add")]
    public async Task AddItemToBasketAsync(BasketItem input)
    {
        //todo: check stock count
        await BasketAppService.AddItemToBasketAsync(input);
    }

    [HttpGet]
    [Route("remove")]
    public async Task RemoveItemFromBasketAsync(BasketItem input)
    {
        await BasketAppService.RemoveItemFromBasketAsync(input);
    }

    [HttpGet]
    public async Task<List<BasketItem>> GetBasketItems()
    {
        return await BasketAppService.GetBasketItems();
    }

    [HttpGet("render")]
    public async Task<ViewComponentResult> Render()
    {
        var result = await BasketAppService.GetBasketItems();

        if (!result.Any())
        {
            return ViewComponent("Basket", new BasketViewModel(new List<BasketViewItemModel>()));
        }

        var productsWithDetails = await ProductsPublicAppService.GetListByIdsAsync(new GetListByIdsInput
        {
            Ids = result.Select(x => x.ItemId).ToList()
        });

        return ViewComponent("Basket", new BasketViewModel(
            productsWithDetails.Items.Select(x => new BasketViewItemModel
            {
                Product = x.Product,
                Category = x.Category,
                Amount = result.FirstOrDefault(y => x.Product.Id == y.ItemId)?.Amount ?? 1
            }).ToList()
        ));
    }
}