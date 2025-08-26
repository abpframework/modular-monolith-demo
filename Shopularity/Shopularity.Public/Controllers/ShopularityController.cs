using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Ordering.Orders;
using Shopularity.Ordering.Orders.Public;
using Shopularity.Services.Dtos;
using Shopularity.Services.Orders;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Public.Controllers;

[RemoteService(Name = "Shopularity")]
[Area("shopularity")]
[ControllerName("Shopularity")]
[Route("api/shopularity")]
public class ShopularityController : AbpController, IShopularityAppService
{
    private readonly IShopularityAppService _shopularityAppService;

    public ShopularityController(IShopularityAppService shopularityAppService)
    {
        _shopularityAppService = shopularityAppService;
    }

    [HttpPost]
    [Route("create")]
    public Task CreateOrderAsync(NewOrderInputDto input)
    {
        return _shopularityAppService.CreateOrderAsync(input);
    }

    [HttpPost]
    [Route("cancel")]
    public Task CancelOrderAsync(Guid id)
    {
        return _shopularityAppService.CancelOrderAsync(id);
    }

    [HttpGet]
    [Route("orders")]
    public Task<ListResultDto<OrderPublicDto>> GetOrdersAsync()
    {
        return _shopularityAppService.GetOrdersAsync();
    }
}