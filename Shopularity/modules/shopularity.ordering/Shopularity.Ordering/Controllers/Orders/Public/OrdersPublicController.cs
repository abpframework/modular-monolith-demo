using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Ordering.Services.Orders.Admin;
using Shopularity.Ordering.Services.Orders.Public;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Ordering.Controllers.Orders.Public;

[RemoteService(Name = "Ordering")]
[Area("ordering")]
[ControllerName("OrderPublic")]
[Route("api/ordering/public/orders")]
public class OrdersPublicController : AbpController, IOrdersPublicAppService
{
    private readonly IOrdersPublicAppService _ordersPublicAppService;

    public OrdersPublicController(IOrdersPublicAppService ordersPublicAppService)
    {
        _ordersPublicAppService = ordersPublicAppService;
    }

    [HttpPost]
    [Route("create")]
    public Task<OrderDto> CreateAsync(OrderCreatePublicDto input)
    {
        return _ordersPublicAppService.CreateAsync(input);
    }

    [HttpGet]
    public Task<ListResultDto<OrderPublicDto>> GetListAsync()
    {
        return _ordersPublicAppService.GetListAsync();
    }

    [HttpPost]
    [Route("cancel")]
    public Task CancelAsync(Guid id)
    {
        return _ordersPublicAppService.CancelAsync(id);
    }
}