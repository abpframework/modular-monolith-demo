using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Shopularity.Ordering.OrderLines;
// TODO: Merge with IOrderAppService, not a separate service
public interface IOrderLinesAppService : IApplicationService
{

    Task<PagedResultDto<OrderLineDto>> GetListByOrderIdAsync(GetOrderLineListInput input);
}