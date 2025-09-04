using Asp.Versioning;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace Shopularity.Ordering.OrderLines;

[RemoteService(Name = "Ordering")]
[Area("ordering")]
[ControllerName("OrderLine")]
[Route("api/ordering/order-lines")]
public class OrderLineController : AbpController, IOrderLinesAppService
{
    protected IOrderLinesAppService _orderLinesAppService;

    public OrderLineController(IOrderLinesAppService orderLinesAppService)
    {
        _orderLinesAppService = orderLinesAppService;
    }

    [HttpGet]
    [Route("by-order")]
    public virtual Task<PagedResultDto<OrderLineDto>> GetListByOrderIdAsync(GetOrderLineListInput input)
    {
        return _orderLinesAppService.GetListByOrderIdAsync(input);
    }
}