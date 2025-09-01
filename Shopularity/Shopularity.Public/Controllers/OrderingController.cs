using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Ordering.Orders;
using Shopularity.Ordering.Orders.Public;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Public.Controllers;

[RemoteService(Name = "Ordering")]
[Area("ordering")]
[ControllerName("OrderPublic")]
[Route("api/ordering/public/orders")]
public class OrderingController : AbpController, IOrdersPublicAppService
{
    protected IOrdersPublicAppService OrdersPublicAppService;

    public OrderingController(IOrdersPublicAppService ordersPublicAppService)
    {
        OrdersPublicAppService = ordersPublicAppService;
    }

    [HttpPost]
    [Route("create")]
    public Task<OrderDto> CreateAsync(OrderCreatePublicDto input)
    {
        return OrdersPublicAppService.CreateAsync(input);
    }

    [HttpGet]
    public Task<ListResultDto<OrderPublicDto>> GetListAsync()
    {
        return OrdersPublicAppService.GetListAsync();
    }

    [HttpPost]
    [Route("cancel")]
    public Task CancelAsync(Guid id)
    {
        return OrdersPublicAppService.CancelAsync(id);
    }
}