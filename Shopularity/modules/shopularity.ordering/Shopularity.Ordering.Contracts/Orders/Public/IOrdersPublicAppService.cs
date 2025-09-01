using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Shopularity.Ordering.Orders.Public;

public interface IOrdersPublicAppService : IApplicationService
{
    Task<OrderDto> CreateAsync(OrderCreatePublicDto input);
    
    Task<ListResultDto<OrderPublicDto>> GetListAsync();
    
    Task CancelAsync(Guid id);
}