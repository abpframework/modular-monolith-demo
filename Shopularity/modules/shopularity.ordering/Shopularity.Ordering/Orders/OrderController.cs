using Asp.Versioning;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Shopularity.Ordering.Orders;
using Volo.Abp.Content;
using Shopularity.Ordering.Shared;

namespace Shopularity.Ordering.Orders
{
    [RemoteService(Name = "Ordering")]
    [Area("ordering")]
    [ControllerName("Order")]
    [Route("api/ordering/orders")]
    public class OrderController : AbpController, IOrdersAppService
    {
        protected IOrdersAppService _ordersAppService;

        public OrderController(IOrdersAppService ordersAppService)
        {
            _ordersAppService = ordersAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<OrderDto>> GetListAsync(GetOrdersInput input)
        {
            return _ordersAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<OrderDto> GetAsync(Guid id)
        {
            return _ordersAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<OrderDto> CreateAsync(OrderCreateDto input)
        {
            return _ordersAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<OrderDto> UpdateAsync(Guid id, OrderUpdateDto input)
        {
            return _ordersAppService.UpdateAsync(id, input);
        }

        [HttpPut]
        [Route("shipped/{id}")]
        public virtual Task<OrderDto> SetShippingInfoAsync(Guid id, SetShippingInfoInput input)
        {
            return _ordersAppService.SetShippingInfoAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _ordersAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(OrderExcelDownloadDto input)
        {
            return _ordersAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public virtual Task<Shopularity.Ordering.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _ordersAppService.GetDownloadTokenAsync();
        }

    }
}