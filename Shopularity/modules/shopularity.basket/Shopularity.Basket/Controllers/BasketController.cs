using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Basket.Services;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Basket.Controllers;

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
    public async Task AddItemToBasket(BasketItem input)
    {
        await BasketAppService.AddItemToBasket(input);
    }

    [HttpGet]
    [Route("remove")]
    public async Task RemoveItemFromBasket(BasketItem input)
    {
        await BasketAppService.RemoveItemFromBasket(input);
    }

    [HttpGet]
    public async Task<List<BasketItem>> GetBasketItems()
    {
        return await BasketAppService.GetBasketItems();
    }
}