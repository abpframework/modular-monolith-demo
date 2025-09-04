using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Basket.Domain;
using Shopularity.Basket.Services;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
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
    public async Task AddItemAsync(AddBasketItemInput input)
    {
        await BasketAppService.AddItemAsync(input);
    }

    [HttpGet]
    [Route("remove")]
    public async Task RemoveItemAsync(RemoveBasketItemInput input)
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
}