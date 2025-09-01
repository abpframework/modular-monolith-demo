using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Shopularity.Ordering.OrderLines;

public interface IOrderLinesAppService : IApplicationService
{

    Task<PagedResultDto<OrderLineDto>> GetListByOrderIdAsync(GetOrderLineListInput input);

    Task<PagedResultDto<OrderLineDto>> GetListAsync(GetOrderLinesInput input);

    Task<OrderLineDto> GetAsync(Guid id);
}