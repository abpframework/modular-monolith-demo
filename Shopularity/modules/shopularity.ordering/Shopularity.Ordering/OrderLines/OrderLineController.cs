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

    [HttpGet]
    public virtual Task<PagedResultDto<OrderLineDto>> GetListAsync(GetOrderLinesInput input)
    {
        return _orderLinesAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<OrderLineDto> GetAsync(Guid id)
    {
        return _orderLinesAppService.GetAsync(id);
    }
}