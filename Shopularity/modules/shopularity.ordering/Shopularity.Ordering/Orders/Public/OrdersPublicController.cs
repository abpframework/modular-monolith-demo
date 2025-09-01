using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Ordering.Orders.Public;

[RemoteService(Name = "Ordering")]
[Area("ordering")]
[ControllerName("OrderPublic")]
[Route("api/ordering/public/orders")]
public class OrdersPublicController : AbpController, IOrdersPublicAppService
{
    protected IOrdersPublicAppService OrdersPublicAppService;

    public OrdersPublicController(IOrdersPublicAppService ordersPublicAppService)
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