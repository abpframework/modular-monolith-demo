using Shopularity.Ordering.Orders;
using Shopularity.Services.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Shopularity.Services.Orders;

public interface IOrderingPublicAppService : IApplicationService
{
    Task CreateOrderAsync(NewOrderInputDto input);
    
    Task<PagedResultDto<OrderDto>> GetOrdersAsync();
}