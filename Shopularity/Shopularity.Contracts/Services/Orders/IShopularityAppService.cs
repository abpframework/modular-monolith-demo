using Shopularity.Ordering.Orders;
using Shopularity.Ordering.Orders.Public;
using Shopularity.Services.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Shopularity.Services.Orders;

public interface IShopularityAppService : IApplicationService
{
    Task CreateOrderAsync(NewOrderInputDto input);

    Task CancelOrderAsync(Guid id);
    
    Task<ListResultDto<OrderPublicDto>> GetOrdersAsync();
}