/*using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Ordering.Orders;
using Shopularity.Services.Dtos;
using Shopularity.Services.Orders;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Shopularity.Controllers;

[Route("api/orders/ordering")]
public class OrderingPublicController : AbpController, IOrderingPublicAppService
{
    protected IOrderingPublicAppService OrdersPublicAppService;

    public OrderingPublicController(IOrderingPublicAppService ordersPublicAppService)
    {
        OrdersPublicAppService = ordersPublicAppService;
    }

    [HttpPost]
    [Route("create")]
    public Task CreateOrderAsync(NewOrderInputDto input)
    {
        return OrdersPublicAppService.CreateOrderAsync(input);
    }

    [HttpGet]
    public Task<PagedResultDto<OrderDto>> GetOrdersAsync()
    {
        return OrdersPublicAppService.GetOrdersAsync();
    }
}*/