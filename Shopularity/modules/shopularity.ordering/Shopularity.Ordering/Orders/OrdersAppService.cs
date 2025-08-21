using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Shopularity.Ordering.Permissions;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace Shopularity.Ordering.Orders
{
    [RemoteService(IsEnabled = false)]
    [Authorize(OrderingPermissions.Orders.Default)]
    public class OrdersAppService : OrderingAppService, IOrdersAppService
    {
        protected IDistributedCache<OrderDownloadTokenCacheItem, string> _downloadTokenCache;
        protected IOrderRepository _orderRepository;
        protected OrderManager _orderManager;

        public OrdersAppService(IOrderRepository orderRepository, OrderManager orderManager, IDistributedCache<OrderDownloadTokenCacheItem, string> downloadTokenCache)
        {
            _downloadTokenCache = downloadTokenCache;
            _orderRepository = orderRepository;
            _orderManager = orderManager;
        }

        public virtual async Task<PagedResultDto<OrderDto>> GetListAsync(GetOrdersInput input)
        {
            var totalCount = await _orderRepository.GetCountAsync(input.FilterText, input.UserId, input.State, input.TotalPriceMin, input.TotalPriceMax, input.ShippingAddress, input.CargoNo);
            var items = await _orderRepository.GetListAsync(input.FilterText, input.UserId, input.State, input.TotalPriceMin, input.TotalPriceMax, input.ShippingAddress, input.CargoNo, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<OrderDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Order>, List<OrderDto>>(items)
            };
        }

        public virtual async Task<OrderDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Order, OrderDto>(await _orderRepository.GetAsync(id));
        }

        [Authorize(OrderingPermissions.Orders.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _orderRepository.DeleteAsync(id);
        }

        [Authorize(OrderingPermissions.Orders.Create)]
        public virtual async Task<OrderDto> CreateAsync(OrderCreateDto input)
        {

            var order = await _orderManager.CreateAsync(
            Guid.Parse(input.UserId),
            input.State,
            input.TotalPrice,
            input.ShippingAddress
            );

            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        [Authorize(OrderingPermissions.Orders.Edit)]
        public virtual async Task<OrderDto> UpdateAsync(Guid id, OrderUpdateDto input)
        {

            var order = await _orderManager.UpdateAsync(
            id,
            input.State, input.ShippingAddress, input.CargoNo, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(OrderExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _orderRepository.GetListAsync(input.FilterText, input.UserId, input.State, input.TotalPriceMin, input.TotalPriceMax, input.ShippingAddress, input.CargoNo);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Order>, List<OrderExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Orders.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public virtual async Task<Shopularity.Ordering.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new OrderDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new Shopularity.Ordering.Shared.DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}