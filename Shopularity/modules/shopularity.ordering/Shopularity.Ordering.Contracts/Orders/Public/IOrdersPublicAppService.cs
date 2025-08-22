using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Shopularity.Ordering.Orders.Public;

public interface IOrdersPublicAppService : IApplicationService
{
    Task<OrderDto> CreateAsync(OrderCreatePublicDto input);
}