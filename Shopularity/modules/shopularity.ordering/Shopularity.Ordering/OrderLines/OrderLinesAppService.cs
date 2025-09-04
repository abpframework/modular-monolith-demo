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

    public OrderLinesAppService(IOrderLineRepository orderLineRepository)
    {

        _orderLineRepository = orderLineRepository;
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
}