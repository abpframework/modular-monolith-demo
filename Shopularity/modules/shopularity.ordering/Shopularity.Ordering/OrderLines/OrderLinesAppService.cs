using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Shopularity.Ordering.Permissions;

namespace Shopularity.Ordering.OrderLines;

[RemoteService(IsEnabled = false)]
[Authorize(OrderingPermissions.OrderLines.Default)]
public class OrderLinesAppService : OrderingAppService, IOrderLinesAppService
{
    protected IOrderLineRepository _orderLineRepository;
    protected OrderLineManager _orderLineManager;

    public OrderLinesAppService(IOrderLineRepository orderLineRepository, OrderLineManager orderLineManager)
    {

        _orderLineRepository = orderLineRepository;
        _orderLineManager = orderLineManager;
    }

    public virtual async Task<PagedResultDto<OrderLineDto>> GetListByOrderIdAsync(GetOrderLineListInput input)
    {
        var orderLines = await _orderLineRepository.GetListByOrderIdAsync(
            input.OrderId,
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount);

        return new PagedResultDto<OrderLineDto>
        {
            TotalCount = await _orderLineRepository.GetCountByOrderIdAsync(input.OrderId),
            Items = ObjectMapper.Map<List<OrderLine>, List<OrderLineDto>>(orderLines)
        };
    }

    public virtual async Task<PagedResultDto<OrderLineDto>> GetListAsync(GetOrderLinesInput input)
    {
        var totalCount = await _orderLineRepository.GetCountAsync(input.FilterText, input.ProductId, input.Name, input.AmountMin, input.AmountMax, input.TotalPriceMin, input.TotalPriceMax);
        var items = await _orderLineRepository.GetListAsync(input.FilterText, input.ProductId, input.Name, input.AmountMin, input.AmountMax, input.TotalPriceMin, input.TotalPriceMax, input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<OrderLineDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<OrderLine>, List<OrderLineDto>>(items)
        };
    }

    public virtual async Task<OrderLineDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<OrderLine, OrderLineDto>(await _orderLineRepository.GetAsync(id));
    }

    [Authorize(OrderingPermissions.OrderLines.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _orderLineRepository.DeleteAsync(id);
    }

    [Authorize(OrderingPermissions.OrderLines.Create)]
    public virtual async Task<OrderLineDto> CreateAsync(OrderLineCreateDto input)
    {
        var orderLine = await _orderLineManager.CreateAsync(
            input.OrderId,
            input.ProductId,
            input.Price,
            input.Amount,
            input.TotalPrice,
            input.Name
        );

        return ObjectMapper.Map<OrderLine, OrderLineDto>(orderLine);
    }

    [Authorize(OrderingPermissions.OrderLines.Edit)]
    public virtual async Task<OrderLineDto> UpdateAsync(Guid id, OrderLineUpdateDto input)
    {

        var orderLine = await _orderLineManager.UpdateAsync(
            id,
            input.OrderId,
            input.Name
        );

        return ObjectMapper.Map<OrderLine, OrderLineDto>(orderLine);
    }
}