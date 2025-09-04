using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
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
using Shopularity.Ordering.OrderLines;
using Shopularity.Ordering.Orders.Events;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity.Integration;
using Volo.Abp.Users;

namespace Shopularity.Ordering.Orders;

[RemoteService(IsEnabled = false)]
[Authorize(OrderingPermissions.Orders.Default)]
public class OrdersAppService : OrderingAppService, IOrdersAppService
{
    protected IDistributedCache<OrderDownloadTokenCacheItem, string> _downloadTokenCache;
    private readonly IIdentityUserIntegrationService _userIntegrationService;
    private readonly IBackgroundJobManager _backgroundJobManager;
    private readonly IDistributedEventBus _eventBus;
    protected IOrderRepository _orderRepository;

    public OrdersAppService(
        IOrderRepository orderRepository,
        IDistributedCache<OrderDownloadTokenCacheItem, string> downloadTokenCache,
        IIdentityUserIntegrationService userIntegrationService,
        IBackgroundJobManager backgroundJobManager,
        IDistributedEventBus eventBus)
    {
        _downloadTokenCache = downloadTokenCache;
        _userIntegrationService = userIntegrationService;
        _backgroundJobManager = backgroundJobManager;
        _eventBus = eventBus;
        _orderRepository = orderRepository;
    }

    public virtual async Task<PagedResultDto<OrderDto>> GetListAsync(GetOrdersInput input)
    {
        var targetUser = await FindFilterTargetUser(input.Username);

        if (!input.Username.IsNullOrWhiteSpace() && targetUser == null)
        {
            return new PagedResultDto<OrderDto>();
        }
        
        var totalCount = await _orderRepository.GetCountAsync(targetUser?.Id, input.State, input.TotalPriceMin, input.TotalPriceMax, input.ShippingAddress, input.CargoNo);
        var items = await _orderRepository.GetListAsync(targetUser?.Id, input.State, input.TotalPriceMin, input.TotalPriceMax, input.ShippingAddress, input.CargoNo, input.Sorting, input.MaxResultCount, input.SkipCount);
        var itemsDto = ObjectMapper.Map<List<Order>, List<OrderDto>>(items);

        foreach (var itemDto in itemsDto)
        {
            var user = targetUser ?? await _userIntegrationService.FindByIdAsync(itemDto.UserId);
            itemDto.Username = user.UserName;
        }
        
        return new PagedResultDto<OrderDto>
        {
            TotalCount = totalCount,
            Items = itemsDto
        };
    }

    private async Task<UserData?> FindFilterTargetUser(string? username)
    {
        UserData? targetUser;
        if (!username.IsNullOrWhiteSpace())
        {
            targetUser = await _userIntegrationService.FindByUserNameAsync(username);
        }
        else
        {
            targetUser = null;
        }

        return targetUser;
    }

    public virtual async Task<OrderDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Order, OrderDto>(await _orderRepository.GetAsync(id));
    }

    [Authorize(OrderingPermissions.Orders.Edit)]
    public virtual async Task<OrderDto> UpdateAsync(Guid id, OrderUpdateDto input)
    {
        var order = await _orderRepository.GetAsync(id);

        order.ShippingAddress = input.ShippingAddress;

        order.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        return ObjectMapper.Map<Order, OrderDto>(order);
    }

    [Authorize(OrderingPermissions.Orders.SetShippingInfo)]
    public virtual async Task<OrderDto> SetShippingInfoAsync(Guid id, SetShippingInfoInput input)
    {
        var order = await _orderRepository.GetAsync(id);

        if (order.State != OrderState.Processing)
        {            
            throw new BusinessException(OrderingErrorCodes.OrderIsNotAvailableYetForShipping);
        }
            
        order.CargoNo = input.CargoNo;
        order.State = OrderState.Shipped;

        order = await _orderRepository.UpdateAsync(order);
        
        await _backgroundJobManager.EnqueueAsync(new OrderFakeStateJob.OrderFakeStateJobArgs
            {
                OrderId = order.Id,
                State = OrderState.Completed
            },
            delay: TimeSpan.FromSeconds(60));
            
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

        var targetUser = await FindFilterTargetUser(input.Username);

        var itemsMapped = new List<OrderExcelDto>();
        if (input.Username.IsNullOrWhiteSpace() || targetUser != null)
        {
            var items = await _orderRepository.GetListAsync(targetUser?.Id, input.State, input.TotalPriceMin,
                input.TotalPriceMax, input.ShippingAddress, input.CargoNo);
            itemsMapped = ObjectMapper.Map<List<Order>, List<OrderExcelDto>>(items);

            foreach (var itemDto in itemsMapped)
            {
                var user = targetUser ?? await _userIntegrationService.FindByIdAsync(itemDto.UserId);
                itemDto.Username = user.UserName;
            }
        }
        
        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(itemsMapped);
        memoryStream.Seek(0, SeekOrigin.Begin);

        return new RemoteStreamContent(memoryStream, "Orders.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    public virtual async Task<Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");

        await _downloadTokenCache.SetAsync(
            token,
            new OrderDownloadTokenCacheItem { Token = token },
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

        return new Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }

    public virtual async Task<PagedResultDto<OrderLineDto>> GetOrderLineListAsync(GetOrderLineListInput input)
    {
        var order= await _orderRepository.GetAsync(input.OrderId, includeDetails: true);

        var orderLines = order.OrderLines
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();
        
        return new PagedResultDto<OrderLineDto>
        {
            TotalCount = order.OrderLines.Count,
            Items = ObjectMapper.Map<List<OrderLine>, List<OrderLineDto>>(orderLines)
        };
    }
}