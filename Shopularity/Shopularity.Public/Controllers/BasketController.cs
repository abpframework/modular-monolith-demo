using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Basket.Services;
using Shopularity.Catalog.Products.Public;
using Shopularity.Public.Components.Basket;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Public.Controllers;

[RemoteService(Name = "Catalog")]
[Area("catalog")]
[ControllerName("Basket")]
[Route("api/basket/basket")]
public class BasketController : AbpController, IBasketAppService
{
    protected IBasketAppService BasketAppService;

    public BasketController(IBasketAppService basketAppService)
    {
        BasketAppService = basketAppService;
    }

    [HttpGet]
    [Route("add")]
    public async Task AddItemAsync(BasketItem input)
    {
        await BasketAppService.AddItemAsync(input);
    }

    [HttpGet]
    [Route("remove")]
    public async Task RemoveItemAsync(BasketItem input)
    {
        await BasketAppService.RemoveItemAsync(input);
    }

    [HttpGet]
    public async Task<ListResultDto<BasketItemDto>> GetItemsAsync()
    {
        return await BasketAppService.GetItemsAsync();
    }

    [HttpGet]
    [Route("count")]
    public async Task<int> GetCountOfItemsAsync()
    {
        return await BasketAppService.GetCountOfItemsAsync();
    }

    [HttpGet("render")]
    public async Task<ViewComponentResult> Render()
    {
        var basket = await BasketAppService.GetItemsAsync();

        if (!basket.Items.Any())
        {
            return ViewComponent("Basket", new BasketViewModel(new List<BasketViewItemModel>()));
        }

        var items = new List<BasketViewItemModel>();

        foreach (var item in basket.Items)
        {
            var newItem = ObjectMapper.Map<ProductPublicDto, BasketViewItemModel>(item.Product);
            newItem.Amount = item.Amount;
            items.Add(newItem);
        }
        
        return ViewComponent("Basket", new BasketViewModel(items));
    }
}