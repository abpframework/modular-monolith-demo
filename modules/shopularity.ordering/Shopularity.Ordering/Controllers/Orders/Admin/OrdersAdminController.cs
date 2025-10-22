using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Shopularity.Ordering.Services.Orders.Admin;
using Shopularity.Ordering.Services.Orders.OrderLines;
using Shopularity.Ordering.Shared;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace Shopularity.Ordering.Controllers.Orders.Admin;

[RemoteService(Name = "Ordering")]
[Area("ordering")]
[ControllerName("Order")]
[Route("api/ordering/orders")]
public class OrdersAdminController : AbpController, IOrdersAdminAppService
{
    private readonly IOrdersAdminAppService _ordersAdminAppService;

    public OrdersAdminController(IOrdersAdminAppService ordersAdminAppService)
    {
        _ordersAdminAppService = ordersAdminAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<OrderDto>> GetListAsync(GetOrdersInput input)
    {
        return _ordersAdminAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<OrderDto> GetAsync(Guid id)
    {
        return _ordersAdminAppService.GetAsync(id);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<OrderDto> SetShipmentAddressInfoAsync(Guid id, OrderUpdateDto input)
    {
        return _ordersAdminAppService.SetShipmentAddressInfoAsync(id, input);
    }

    [HttpPut]
    [Route("shipped/{id}")]
    public virtual Task<OrderDto> SetShipmentCargoNoAsync(Guid id, SetShipmentCargoNoInput input)
    {
        return _ordersAdminAppService.SetShipmentCargoNoAsync(id, input);
    }

    [HttpGet]
    [Route("as-excel-file")]
    public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(OrderExcelDownloadDto input)
    {
        return _ordersAdminAppService.GetListAsExcelFileAsync(input);
    }

    [HttpGet]
    [Route("download-token")]
    public virtual Task<DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        return _ordersAdminAppService.GetDownloadTokenAsync();
    }

    [HttpGet]
    [Route("lines")]
    public virtual Task<PagedResultDto<OrderLineDto>> GetOrderLineListAsync(GetOrderLineListInput input)
    {
        return _ordersAdminAppService.GetOrderLineListAsync(input);
    }
}